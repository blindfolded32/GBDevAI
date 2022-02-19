using UnityEngine;

public class Enemy : IEnemy
{
    private string _name;
    private int _enemyPower;

    private int _moneyPlayer;
    private int _healthPlayer;
    private int _powerPlayer;
    private int _playerCrimeLevel;

    public Enemy(string name, int enemyPower)
    {
        _name = name;
        _enemyPower = enemyPower;
    }

    public void Update(DataPlayer dataPlayer, DataType dataType)
    {
        switch (dataType)
        {
            case DataType.Health:
                _healthPlayer = dataPlayer.CountHealth;
                break;

            case DataType.Money:
                _moneyPlayer = dataPlayer.CountMoney;
                break;

            case DataType.Power:
                _powerPlayer = dataPlayer.CountPower;
                break;
            
            case DataType.CrimeLevel:
                _playerCrimeLevel = dataPlayer.CrimeLevel;
                break;
        }

        Debug.Log($"Update {_name}, change {dataType}");
    }

    public int Power
    {
        get
        {
            var power = _enemyPower + _moneyPlayer + _healthPlayer - _powerPlayer - _playerCrimeLevel;
            if (power < 0) power = 0;
            return power;
        }
    }
}
