namespace Domain.Common
{
    public class AuditEntityBase<T> : EntityBase<T>
    {
        public DateTime DateAndTimeOfCreation { get; private set; }
        public DateTime? DateAndTimeOfDeletion { get; private set; }
        public DateTime? DateAndTimeOfLastModification { get; private set; }
        public int UserIdOfCreation { get; private set; }
        public int? UserIdOfDeletion { get; private set; }
        public int? UserIdOfLastModification { get; private set; }
        public AuditEntityBase() { 
        }
        public AuditEntityBase(
            DateTime dateAndTimeOfCreation,
            DateTime? dateAndTimeOfDeletion,
            DateTime? dateAndTimeOfLastModification,
            int userIdOfCreation,
            int? userIdOfDeletion,
            int? userIdOfLastModification)
        {
            DateAndTimeOfCreation = dateAndTimeOfCreation;
            DateAndTimeOfDeletion = dateAndTimeOfDeletion ?? null;
            DateAndTimeOfLastModification = dateAndTimeOfLastModification ?? null;
            UserIdOfCreation = userIdOfCreation;
            UserIdOfDeletion = userIdOfDeletion ?? null;
            UserIdOfLastModification = userIdOfLastModification ?? null;
        }
        public AuditEntityBase(int userIdOfCreation)
        {
            {
                DateAndTimeOfCreation = DateTime.UtcNow;
                UserIdOfCreation = userIdOfCreation;
            }
        }
        public AuditEntityBase(int? userIdOfLastModification)
        {
            {
                DateAndTimeOfCreation = DateTime.UtcNow;
                UserIdOfLastModification = userIdOfLastModification;
            }
        }

        //public void SetDateAndTImeAndUserIdOfCreation(int userIdOfCreation)
        //{
        //    DateAndTimeOfCreation = DateTime.UtcNow;
        //    UserIdOfCreation = userIdOfCreation;

        //}
        //public void SetDateAndTimeAndUserIdOfDeletion(int userIdOfDeletion)
        //{
        //    DateAndTimeOfDeletion = DateTime.UtcNow;
        //    UserIdOfDeletion = userIdOfDeletion;

        //}
        //public void SetDateAndTimeAndUserIdOfLastModification(int userIdOfLastModification)
        //{
        //    DateAndTimeOfLastModification = DateTime.UtcNow;
        //    UserIdOfLastModification = userIdOfLastModification;
        //}
    }
}
