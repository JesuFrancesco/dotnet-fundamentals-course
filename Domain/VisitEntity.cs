namespace Domain
{
    public class VisitEntity
    {
        public Guid Id { get; private set; }
        public Guid PersonId { get; private set; }
        public DateTime EntryTime { get; private set; }
        public DateTime? ExitTime { get; private set; }
        public PersonEntity? Person { get; private set; }

        public bool IsActive => ExitTime == null;
        public TimeSpan? Duration => ExitTime.HasValue ? ExitTime.Value - EntryTime : null;


        public VisitEntity(Guid personId, DateTime? entryTime = null)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId), "El ID de la persona no puede estar vacío");
            }
            Id = Guid.NewGuid();
            PersonId = personId;
            EntryTime = entryTime ?? DateTime.UtcNow;
        }

        // evil ef constructor
        private VisitEntity()
        {
        }

        public void RegisterExit(DateTime? exitTime = null)
        {
            if (ExitTime.HasValue) { throw new InvalidOperationException("La visita ya tiene salida registrada"); }

            var exit = exitTime ?? DateTime.UtcNow;
            if (exit <= EntryTime)
            {
                throw new ArgumentException("La hora de slaida debe ser posterior a la hora de entrada", nameof(exit));
            }

            ExitTime = exit;
        }
    }
}
