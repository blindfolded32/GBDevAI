using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FightWindowView : MonoBehaviour
{
    [SerializeField] private TMP_Text _countMoneyText;
    [SerializeField] private TMP_Text _countHealthText;
    [SerializeField] private TMP_Text _countPowerText;
    [SerializeField] private TMP_Text _countPowerEnemyText;
    [SerializeField] private Button _addMoneyButton;
    [SerializeField] private Button _minusMoneyButton;
    [SerializeField] private Button _addHealthButton;
    [SerializeField] private Button _minusHealthButton;
    [SerializeField] private Button _addPowerButton;
    [SerializeField] private Button _minusPowerButton;

    [SerializeField] private Toggle _addKnifeToggle;
    [SerializeField] private Toggle _addGunToggle;
    [SerializeField] private Button _removeWeaponButton;

    [SerializeField] private Button _addCrimeLevelButton;
    [SerializeField] private Button _minusCrimeLevelButton;
    
    [SerializeField] private Button _fightButton;
    [SerializeField] private Button _skipFightButton;
    
    [SerializeField] private TMP_Text _fightResultText;
    [SerializeField] private TMP_Text _crimeLevelText;

    [SerializeField] private  float _knifePowerModifier;
    [SerializeField] private  float _gunPowerModifier;

    private const int CrimeWaterLine = 2;
    private const int _defaultEnemyPower = 1;
    private int _enemyCrimeLevel=0;
    
    private Enemy _enemy;
    
    private Money _money;
    private Health _health;
    private Power _power;
    private CrimeLevel _crimeLevel;
    private AttackType _enemyAttackType = AttackType.Melee;
    
    

    private int _allCountMoneyPlayer;
    private int _allCountHealthPlayer;
    private int _allCountPowerPlayer;

    private void Start()
    {
        _enemy = new Enemy("Flappy", _defaultEnemyPower);
        _countPowerEnemyText.text = $"Enemy power: {_enemy.Power}";


        _money = new Money(nameof(Money));
        _money.Attach(_enemy);

        _health = new Health(nameof(Health));
        _health.Attach(_enemy);

        _power = new Power(nameof(Power));
        _power.Attach(_enemy);
        
        _crimeLevel = new CrimeLevel(nameof(CrimeLevel));
        _crimeLevel.Attach(_enemy);

        _addMoneyButton.onClick.AddListener(() => ChangeMoney(true));
        _minusMoneyButton.onClick.AddListener(() => ChangeMoney(false));

        _addHealthButton.onClick.AddListener(() => ChangeHealth(true));
        _minusHealthButton.onClick.AddListener(() => ChangeHealth(false));

        _addPowerButton.onClick.AddListener(() => ChangePower(true));
        _minusPowerButton.onClick.AddListener(() => ChangePower(false));
        
        _addCrimeLevelButton.onClick.AddListener(() => ChangeCrimeLevel(true));
        _minusCrimeLevelButton.onClick.AddListener(() => ChangeCrimeLevel(false));

        _addKnifeToggle.onValueChanged.AddListener(isSelected => _enemyAttackType = isSelected ? ChangeEnemyWeapon(AttackType.Knife) : ChangeEnemyWeapon(AttackType.Melee));
       
        _addKnifeToggle.onValueChanged.AddListener(isSelected => _enemyAttackType = isSelected ? ChangeEnemyWeapon(AttackType.Gun) : ChangeEnemyWeapon(AttackType.Melee));
        _removeWeaponButton.onClick.AddListener(() =>
        {
            ChangeEnemyWeapon(AttackType.Melee);
            _addGunToggle.isOn = false;
            _addKnifeToggle.isOn = false;
        });

        
        _skipFightButton.onClick.AddListener(SkipFight);
        _fightButton.onClick.AddListener(()=>Fight(_enemyAttackType));
    }

    private AttackType ChangeEnemyWeapon(AttackType attackType) => _enemyAttackType = attackType;
    private void OnDestroy()
    {
        _addMoneyButton.onClick.RemoveAllListeners();
        _minusMoneyButton.onClick.RemoveAllListeners();

        _addHealthButton.onClick.RemoveAllListeners();
        _minusHealthButton.onClick.RemoveAllListeners();

        _addPowerButton.onClick.RemoveAllListeners();
        _minusPowerButton.onClick.RemoveAllListeners();
        
        _addGunToggle.onValueChanged.RemoveAllListeners();
        _addKnifeToggle.onValueChanged.RemoveAllListeners();
        _removeWeaponButton.onClick.RemoveAllListeners();

        _fightButton.onClick.RemoveAllListeners();
        _skipFightButton.onClick.RemoveAllListeners();

        _money.Detach(_enemy);
        _health.Detach(_enemy);
        _power.Detach(_enemy);
    }
    private void Fight(AttackType type = AttackType.Melee)
    {
        switch (type)
        {
            case AttackType.Melee:
                _fightResultText.text = _allCountPowerPlayer >= _enemy.Power ? $"You defeated the enemy in {type}" : $"You were biten";
                break;
            case AttackType.Knife:
                _fightResultText.text = _allCountPowerPlayer >= _enemy.Power * _knifePowerModifier ? $"You defeated the enemy with {type}" : $"You were hit with a knife";
                break;
            case AttackType.Gun:
                _fightResultText.text = _allCountPowerPlayer >= _enemy.Power * _gunPowerModifier ? $"You defeated the enemy with {type}" : $"You were shot";
                break;
        }
    }

    private void SkipFight()
    {
        _fightResultText.text = "You Succesfully avoid fight";
    }
    private void ChangePower(bool isAddCount)
    {
        if (isAddCount)
            _allCountPowerPlayer++;
        else
            _allCountPowerPlayer--;

        ChangeDataWindow(_allCountPowerPlayer, DataType.Power);
    }
    private void ChangeHealth(bool isAddCount)
    {
        if (isAddCount)
            _allCountHealthPlayer++;
        else
            _allCountHealthPlayer--;

        ChangeDataWindow(_allCountHealthPlayer, DataType.Health);
    }
    private void ChangeMoney(bool isAddCount)
    {
        if (isAddCount)
            _allCountMoneyPlayer++;
        else
            _allCountMoneyPlayer--;

        ChangeDataWindow(_allCountMoneyPlayer, DataType.Money);
    }
    
    private void ChangeCrimeLevel(bool isAddCount)
    {
        if (isAddCount)
            _enemyCrimeLevel++;
        else
            _enemyCrimeLevel--;

        ChangeDataWindow(_enemyCrimeLevel, DataType.CrimeLevel);
    }
    
    private void ChangeSkipButtonAvailable(int value)
    {
        _skipFightButton.interactable = value <= CrimeWaterLine;
    }
    
    private void ChangeDataWindow(int countChangeData, DataType dataType)
    {
        switch (dataType)
        {
            case DataType.Money:
                _countMoneyText.text = $"Player money: {countChangeData}";
                _money.CountMoney = countChangeData;
                break;

            case DataType.Health:
                _countHealthText.text = $"Player health: {countChangeData}";
                _health.CountHealth = countChangeData;
                break;

            case DataType.Power:
                _countPowerText.text = $"Player power: {countChangeData}";
                _power.CountPower = countChangeData;
                break;
            case DataType.CrimeLevel:
                _crimeLevelText.text = $"Enemy crime level: {countChangeData}";
                _enemyCrimeLevel = countChangeData;
                ChangeSkipButtonAvailable(countChangeData);
                break;
        }

        _countPowerEnemyText.text = $"Enemy power: {_enemy.Power}";
    }
}
