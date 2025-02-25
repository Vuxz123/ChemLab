using System;
using System.Collections.Generic;
using System.Linq;
using com.ethnicthv.chemlab.engine.api.mixture;
using com.ethnicthv.chemlab.engine.api.molecule.group;
using com.ethnicthv.chemlab.engine.api.reaction;
using com.ethnicthv.chemlab.engine.molecule;
using com.ethnicthv.chemlab.engine.molecule.group;
using com.ethnicthv.chemlab.engine.reaction;
using com.ethnicthv.chemlab.engine.util;
using com.ethnicthv.util;
using UnityEngine;
using Util = com.ethnicthv.chemlab.engine.mixture.MixtureUtil;

namespace com.ethnicthv.chemlab.engine.mixture
{
    public class Mixture : IMixture
    {
        public static readonly float ImpurityThreshold = 0.1F;

        //Note: float is used to represent the number of moles of the element in mol in the mixture
        private readonly Dictionary<Molecule, float> _mixtureComposition = new();
        private readonly Dictionary<Molecule, float> _states = new();

        private readonly Dictionary<MoleculeGroup, List<Molecule>> _moleculeGroups = new();

        private readonly Dictionary<ReactionResult, float> _reactionResults = new();
        private readonly CustomList<IReactingReaction> _possibleReaction = new();

        private readonly List<Molecule> _novelMolecules = new();

        private float _temperature;
        private bool _equilibrium;
        private bool _boiling;

        private (float, Molecule) _nextHigherBoilingPoint = (float.MaxValue, null);
        private (float, Molecule) _nextLowerBoilingPoint = (0, null);

        private readonly Queue<Molecule> _newMolecules = new();
        private readonly Dictionary<Molecule, int> _moleculesToRemove = new();

        private bool _isMixtureChecked;
        private Color _color;

        public static Mixture CreateMixture()
        {
            return new Mixture();
        }

        public static Mixture Pure(Molecule molecule, float state = 0.0F)
        {
            var mixture = new Mixture();
            if (molecule.GetCharge() == 0)
            {
                mixture.AddMoles(molecule, molecule.GetPureConcentration(),
                    out mixture._isMixtureChecked);
                return mixture;
            }

            var otherIon = molecule.GetCharge() < 0 ? Molecules.SodiumIon : Molecules.Chloride;
            var chargeMagnitude = Math.Abs(molecule.GetCharge());
            mixture.AddMoles(molecule, 1.0F, out mixture._isMixtureChecked);
            mixture.AddMoles(otherIon, chargeMagnitude, out mixture._isMixtureChecked);
#pragma warning disable CS0612 // Type or member is obsolete
            mixture.RecalculateVolume(1000);
#pragma warning restore CS0612 // Type or member is obsolete

            mixture.CheckMixture();
            return mixture;
        }

        private Mixture()
        {
            _temperature = 298.15F;
        }

        public Color GetColor()
        {
            return _color;
        }

        public bool IsBoiling()
        {
            return _boiling;
        }

        public Mixture SetTemperature(float temperature)
        {
            _temperature = temperature;
            foreach (var molecule in _mixtureComposition.Keys)
            {
                if (molecule.GetBoilingPoint() < temperature)
                {
                    _states[molecule] = 1.0F;
                }
                else
                {
                    _states[molecule] = 0.0F;
                }
            }

            return this;
        }

        public void SetState(Molecule molecule, float state)
        {
            if (!(state < 0.0F) && !(state > 1.0F))
            {
                if (GetMoles(molecule) > 0.0F)
                {
                    _states.Add(molecule, state);
                }
            }
            else
            {
                throw new Exception("Molecules can range from entirely liquid (state = 0) to entirely gas (state = 1)");
            }
        }

        public void Tick()
        {
            if (!_isMixtureChecked)
            {
                CheckMixture();
                _isMixtureChecked = true;
            }

            RunReactions();
        }

        public void RemoveMolecule(Molecule molecule)
        {
            Util.RemoveMolecule(molecule, _moleculesToRemove, out _isMixtureChecked);
        }

        public void SetMoles(Molecule molecule, float moles)
        {
            if (moles <= 0.0F)
            {
                throw new Exception(
                    "Set Moles in Mixture must be greater than 0, if you want to remove a molecule, use RemoveMolecule()");
            }

            if (!_mixtureComposition.ContainsKey(molecule))
            {
                Util.AddMolecule(molecule, moles,
                    _newMolecules, GetContent(), _states,
                    _novelMolecules,
                    out _isMixtureChecked);
                return;
            }

            _mixtureComposition[molecule] = moles;
        }

        public float GetMoles(Molecule molecule)
        {
            var t = GetContent();
            return t.TryGetValue(molecule, out var value) ? value : 0;
        }

        public float AddMoles(Molecule molecule, float moles, out bool isMutating)
        {
            var t = Util.AddMoles(molecule, moles,
                _newMolecules, _moleculesToRemove,
                GetContent(), _states, _novelMolecules,
                ref _isMixtureChecked);
            isMutating = _isMixtureChecked;
            return t;
        }

        public IReadOnlyDictionary<Molecule, float> GetMixtureComposition()
        {
            return _mixtureComposition;
        }

        public void ClearMixture()
        {
            _mixtureComposition.Clear();
            _isMixtureChecked = false;
        }

        public void Scale(float volumeIncreaseFactor)
        {
            var keys = _mixtureComposition.Keys.ToList();
            foreach (var key in keys)
            {
                _mixtureComposition[key] /= volumeIncreaseFactor;
            }

            var reactionKeys = _reactionResults.Keys.ToList();
            foreach (var key in reactionKeys)
            {
                _reactionResults[key] /= volumeIncreaseFactor;
            }
        }

        public Phases SeparatePhases(float initialVolume)
        {
            Dictionary<Molecule, float> liquidMoles = new();
            Dictionary<Molecule, float> gasMoles = new();
            Dictionary<Molecule, float> solidMoles = new();

            var newLiquidVolume = 0.0F;
            var newGasVolume = 1.0F;

            var liquidMixture = new Mixture();
            var gasMixture = new Mixture();
            var solidMixture = new Mixture();

            foreach (var (molecule, concentration) in _mixtureComposition)
            {
                if (_states[molecule] < 0)
                {
                    solidMoles[molecule] = concentration * initialVolume;
                    continue;
                }

                var proportionGaseous = _states[molecule];
                var molesOfLiquidMolecule = concentration * (1.0F - proportionGaseous) * initialVolume;
                liquidMoles[molecule] = molesOfLiquidMolecule;
                newLiquidVolume += molesOfLiquidMolecule / molecule.GetPureConcentration();
                gasMoles[molecule] = concentration * proportionGaseous * initialVolume;
            }

            foreach (var (molecule, resultMoles) in liquidMoles)
            {
                if (resultMoles == 0.0F) continue;
                liquidMixture.AddMoles(molecule, resultMoles / newLiquidVolume, out liquidMixture._isMixtureChecked);
                liquidMixture._states[molecule] = 0.0F;
            }

            foreach (var (molecule, resultMoles) in gasMoles)
            {
                if (resultMoles == 0.0F) continue;
                gasMixture.AddMoles(molecule, resultMoles / newGasVolume, out gasMixture._isMixtureChecked);
                gasMixture._states[molecule] = 1.0F;
            }

            foreach (var (molecule, resultMoles) in solidMoles)
            {
                if (resultMoles == 0.0F) continue;
                solidMixture.AddMoles(molecule, resultMoles / initialVolume, out solidMixture._isMixtureChecked);
                solidMixture._states[molecule] = -1.0F;
            }

            foreach (var entry in _reactionResults)
            {
                var resultMoles = entry.Value * initialVolume;
                var newTotalVolume = newLiquidVolume * newGasVolume;
                liquidMixture._reactionResults[entry.Key] = resultMoles / newTotalVolume;
                gasMixture._reactionResults[entry.Key] = resultMoles / newTotalVolume;
            }

            liquidMixture._temperature = _temperature;
            gasMixture._temperature = _temperature;
            solidMixture._temperature = _temperature;
            liquidMixture.CheckMixture();
            gasMixture.CheckMixture();
            solidMixture.CheckMixture();
            liquidMixture._equilibrium = _equilibrium;
            gasMixture._equilibrium = _equilibrium;
            solidMixture._equilibrium = _equilibrium;

            return new Phases(gasMixture, newGasVolume, liquidMixture, newLiquidVolume, solidMixture);
        }

        public static Mixture Mix(Dictionary<Mixture, float> mixtures)
        {
            switch (mixtures.Count)
            {
                case 0:
                    return new Mixture();
                case 1:
                    return mixtures.Keys.GetEnumerator().Current;
            }

            var resultMixture = new Mixture();
            Dictionary<Molecule, float> moleculesAndMoles = new();
            Dictionary<ReactionResult, float> reactionResultsAndMoles = new();
            var totalAmount = 0.0f;
            var totalEnergy = 0.0F;

            foreach (var (mixture, amount) in mixtures)
            {
                totalAmount += amount;

                foreach (var (molecule, concentration) in mixture._mixtureComposition)
                {
                    moleculesAndMoles[molecule] =
                        moleculesAndMoles.GetValueOrDefault(molecule, 0) + concentration * amount;
                    totalEnergy += molecule.GetLatentHeat() * concentration * mixture._states[molecule] * amount;
                    totalEnergy += molecule.GetMolarHeatCapacity() * concentration * mixture._temperature * amount;
                }

                foreach (var entry in mixture._reactionResults)
                {
                    reactionResultsAndMoles[entry.Key] =
                        reactionResultsAndMoles.GetValueOrDefault(entry.Key, 0) + entry.Value * amount;
                }
            }

            foreach (var (molecule, value) in moleculesAndMoles)
            {
                //resultMixture.internalAddMolecule(molecule, value / totalAmount, false);
                MixtureUtil.AddMolecule(molecule, value / totalAmount,
                    resultMixture._newMolecules,
                    resultMixture._mixtureComposition,
                    resultMixture._states,
                    resultMixture._novelMolecules,
                    out resultMixture._isMixtureChecked);
                resultMixture._states[molecule] = 0.0F;
            }

            foreach (var (reactionResult, value) in reactionResultsAndMoles)
            {
                if (reactionResult.GetReaction() != null)
                {
                    resultMixture.IncrementReactionResults(reactionResult.GetReaction(),
                        value / totalAmount);
                }
            }

            resultMixture._temperature = 0.0F;
            resultMixture.UpdateNextBoilingPoints();
            resultMixture.Heat(totalEnergy / totalAmount);

            resultMixture.CheckMixture();
            resultMixture.UpdateColor();
            resultMixture.UpdateNextBoilingPoints();
            return resultMixture;
        }

        public void Heat(float energyDensity)
        {
            var volumetricHeatCapacity = GetVolumetricHeatCapacity();

            if (volumetricHeatCapacity == 0.0F) return;

            var temperatureChange = energyDensity / volumetricHeatCapacity;

            if (temperatureChange == 0.0F) return;
            Molecule molecule;
            float liquidConcentration;
            float energyRequiredToFullyBoil;
            float boiled;
            if (temperatureChange > 0.0F)
            {
                if (_nextHigherBoilingPoint.Item2 != null && _temperature + temperatureChange >=
                    _nextHigherBoilingPoint.Item1)
                {
                    temperatureChange = _nextHigherBoilingPoint.Item1 - _temperature;
                    _temperature += temperatureChange;
                    energyDensity -= temperatureChange * volumetricHeatCapacity;
                    molecule = _nextHigherBoilingPoint.Item2;
                    liquidConcentration = GetMoles(molecule) * (1.0F - _states[molecule]);
                    energyRequiredToFullyBoil = liquidConcentration * molecule.GetLatentHeat();
                    if (energyDensity > energyRequiredToFullyBoil)
                    {
                        _states[molecule] = 1;
                        UpdateNextBoilingPoints(true);
                        _boiling = false;
                        Heat(energyDensity - energyRequiredToFullyBoil);
                    }
                    else
                    {
                        boiled = energyDensity / (molecule.GetLatentHeat() * GetMoles(molecule));

                        if (_states.ContainsKey(molecule))
                        {
                            _states[molecule] += boiled;
                        }
                        else
                        {
                            _states[molecule] = boiled;
                        }

                        _boiling = true;
                    }

                    _equilibrium = false;
                }
                else
                {
                    _temperature += temperatureChange;
                }
            }
            else if (_nextLowerBoilingPoint.Item2 != null &&
                     _temperature + temperatureChange < _nextLowerBoilingPoint.Item1)
            {
                temperatureChange = _nextLowerBoilingPoint.Item1 - _temperature;
                _temperature += temperatureChange;
                energyDensity -= temperatureChange * volumetricHeatCapacity;
                molecule = _nextLowerBoilingPoint.Item2;
                liquidConcentration = GetMoles(molecule) * _states[molecule];
                energyRequiredToFullyBoil = liquidConcentration * molecule.GetLatentHeat();
                if (energyDensity < -energyRequiredToFullyBoil)
                {
                    _states[molecule] = 0;
                    UpdateNextBoilingPoints(true);
                    _boiling = false;
                    Heat(energyDensity + energyRequiredToFullyBoil);
                }
                else
                {
                    boiled = -energyDensity / (molecule.GetLatentHeat() * GetMoles(molecule));
                    if (_states.ContainsKey(molecule))
                    {
                        _states[molecule] = _states[molecule] + 1.0F - boiled - 1.0F;
                    }
                    else
                    {
                        _states[molecule] = 1.0F - boiled;
                    }

                    _boiling = true;
                }

                _equilibrium = false;
            }
            else
            {
                _temperature += temperatureChange;
            }

            _temperature = Math.Max(_temperature, 1.0E-4F);
        }

        public float GetVolumetricHeatCapacity()
        {
            var totalHeatCapacity = 0.0F;

            foreach (var entry in _mixtureComposition)
            {
                totalHeatCapacity += entry.Key.GetMolarHeatCapacity() * entry.Value;
            }

            return totalHeatCapacity;
        }

        public bool IsAtEquilibrium()
        {
            return _equilibrium;
        }

        public void DisturbEquilibrium()
        {
            _equilibrium = false;
        }

        public void UpdateColor()
        {
            var totalColorContribution = 0.0F;
            var totalRed = 0.0F;
            var totalGreen = 0.0F;
            var totalBlue = 0.0F;
            var totalAlpha = 64 / 255f;

            foreach (var entry in _mixtureComposition)
            {
                var color = ColorUtil.GetColor(entry.Key.GetColor());
                var colorContribution = entry.Value * color.a;
                totalColorContribution += colorContribution;
                totalRed += color.r * colorContribution;
                totalGreen += color.g * colorContribution;
                totalBlue += color.b * colorContribution;
                totalAlpha = Mathf.Max(totalAlpha, color.a);
            }

            _color = new Color(totalRed / totalColorContribution,
                totalGreen / totalColorContribution,
                totalBlue / totalColorContribution,
                totalAlpha);
        }

        [Obsolete]
        public int RecalculateVolume(int initialVolume)
        {
            if (_mixtureComposition.Count == 0)
            {
                return 0;
            }

            var initialVolumeInLiters = initialVolume / 1.0f;
            var newVolumeInLiters = 0.0f;
            Dictionary<Molecule, float> molesOfMolecules = new();

            foreach (var (molecule, value) in _mixtureComposition)
            {
                var molesOfMolecule = value * initialVolumeInLiters;
                molesOfMolecules[molecule] = molesOfMolecule;
                newVolumeInLiters += molesOfMolecule / molecule.GetPureConcentration();
            }

            foreach (var entry in molesOfMolecules)
            {
                _mixtureComposition[entry.Key] = entry.Value / newVolumeInLiters;
            }

            Dictionary<ReactionResult, float> resultsCopy = new(_reactionResults);
            foreach (var entry in resultsCopy)
            {
                _reactionResults[entry.Key] = entry.Value * initialVolumeInLiters / newVolumeInLiters;
            }

            return (int)(newVolumeInLiters * 1.0D);
        }

        public void AddToGroup(MoleculeGroup getGroup, Molecule molecule)
        {
            if (!_moleculeGroups.ContainsKey(getGroup)) _moleculeGroups.Add(getGroup, new List<Molecule>());
            _moleculeGroups[getGroup].Add(molecule);
        }

        #region Private Action

        private void CheckMixture()
        {
            foreach (var molecule in _newMolecules)
            {
                GroupDetectingProgram.Instance.CheckMolecule(molecule, out var groups);

                foreach (var group in groups)
                {
                    Debug.Log("Group: " + group);
                    AddToGroup(group, molecule);
                }
            }

            ReactionProgram.Instance.CheckForReaction(GetReactionContext(), in _possibleReaction);
        }

        private void RunReactions()
        {
            Dictionary<Molecule, float> oldContents = new(_mixtureComposition);

            //var reactionTickContext = GetReactionTickContext();
            foreach (var r in _possibleReaction.GetList())
            {
                var molesOfReaction = 0F;

                foreach (var reactant in r.GetReactants())
                {
                    var reactantMolarRatio = r.GetReactantMolarRatio(reactant);
                    var reactantConcentration = GetMoles(reactant);
                    if (reactantConcentration < reactantMolarRatio * molesOfReaction)
                    {
                        molesOfReaction = reactantConcentration / reactantMolarRatio;
                    }
                }

                if (!(molesOfReaction <= 0.0F))
                {
                    _isMixtureChecked |= DoReaction(r, molesOfReaction);
                }
            }

            foreach (var molecule in oldContents.Keys)
            {
                if (Mathf.Approximately(oldContents[molecule], GetMoles(molecule)))
                {
                    _equilibrium = false;
                }
            }
        }

        private bool DoReaction(IReactingReaction reaction, float molesPerLiter)
        {
            var shouldRefreshPossibleReactions = false;
            foreach (var reactant in reaction.GetReactants())
            {
                AddMoles(reactant, -(molesPerLiter * reaction.GetReactantMolarRatio(reactant)),
                    out shouldRefreshPossibleReactions);
            }

            foreach (var product in reaction.GetProducts())
            {
                AddMoles(product, molesPerLiter * reaction.GetProductMolarRatio(product),
                    out shouldRefreshPossibleReactions);
            }

            Heat(-reaction.GetEnthalpyChange() * 1000.0F * molesPerLiter);
            IncrementReactionResults(reaction, molesPerLiter);
            return shouldRefreshPossibleReactions;
        }

        private void IncrementReactionResults(IReactingReaction reaction, float molesPerBucket)
        {
            if (!reaction.HasResult()) return;

            var result = reaction.GetResult();

            if (_reactionResults.ContainsKey(result))
            {
                _reactionResults[result] += molesPerBucket;
            }
            else
            {
                _reactionResults[result] = molesPerBucket;
            }
        }

        private void UpdateNextBoilingPoints(bool ignoreCurrentTemperature = false)
        {
            _nextHigherBoilingPoint = (float.MaxValue, null);
            _nextLowerBoilingPoint = (0, null);
            using var var2 = _mixtureComposition.Keys.GetEnumerator();
            while(true) {
                Molecule molecule;
                float bp;
                do {
                    if (!var2.MoveNext()) {
                        return;
                    }
                    molecule = var2.Current;
                    bp = molecule!.GetBoilingPoint();
                    if ((bp < _temperature || bp == _temperature && !ignoreCurrentTemperature) && bp > _nextLowerBoilingPoint.Item1) {
                        _nextLowerBoilingPoint = (bp, molecule);
                    }
                } while(!(bp > _temperature) && (bp != _temperature || ignoreCurrentTemperature));

                if (bp < _nextHigherBoilingPoint.Item1) {
                    _nextHigherBoilingPoint = (bp, molecule);
                }
            }
        }

        #endregion

        #region Context Builders

        private ReactionTickContext GetReactionTickContext()
        {
            return new ReactionTickContext();
        }

        private ReactionContext GetReactionContext()
        {
            return new ReactionContext(_moleculeGroups, _mixtureComposition);
        }

        #endregion

        private Dictionary<Molecule, float> GetContent()
        {
            return _mixtureComposition;
        }
    }
}