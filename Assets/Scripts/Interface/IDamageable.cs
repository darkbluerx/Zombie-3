using System;

public interface IDamageable
{
    event Action<int> OnTakeDamage;
    event Action OnDeadEvent;

    int MaxHealth { get;}
    int CurrentHealth { get;}

    int TakeDamage(int damage);
}
