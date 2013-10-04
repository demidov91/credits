using System;

namespace Coffee.Entities
{
    public class PassportInfo
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string FullName {
            get
            {
                return string.Format("{0} {1} {2}", Surname, FirstName, Patronymic).Trim();
            }
        }

        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public string PassportNumber { get; set; }

        public string IdentificationNumber { get; set; }
    }
}
