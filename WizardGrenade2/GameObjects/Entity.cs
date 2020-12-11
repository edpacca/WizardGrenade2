namespace WizardGrenade2
{
    public class Entity
    {
        public int Health { get; private set; }
        public bool IsDead { get; private set; }
        public bool JustDied { get; private set; }
        private bool _previousIsDead;
        private bool _currentIsDead;

        public Entity(int startHealth)
        {
            IsDead = false;
            Health = startHealth;
        }

        public void ApplyDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
                IsDead = true;
            }
        }

        public void Update()
        {
            _previousIsDead = _currentIsDead;
            _currentIsDead = IsDead;
            JustDied = _previousIsDead != _currentIsDead && !_previousIsDead;
        }

        public void Kill()
        {
            Health = 0;
            IsDead = true;
        }
    }
}
