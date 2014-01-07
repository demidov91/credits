using System;
using System.ComponentModel.DataAnnotations;

namespace Coffee.Entities
{
    public class PassportInfo: IUpdateable<PassportInfo>
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

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.mm.yyyy}")]
        public DateTime BirthDate { get; set; }

        public DateTime IssueDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.mm.yyyy}")]
        public DateTime ExpireDate { get; set; }

        public string PassportNumber { get; set; }

        public string IdentificationNumber { get; set; }

        public void Update(PassportInfo other) {
            this.BirthDate = other.BirthDate;
            this.ExpireDate = other.ExpireDate;
            this.FirstName = other.FirstName;
            this.Gender = other.Gender;
            this.IdentificationNumber = other.IdentificationNumber;
            this.IssueDate = other.IssueDate;
            this.PassportNumber = other.PassportNumber;
            this.Patronymic = other.Patronymic;
            this.Surname = other.Surname;
        }

        public override int GetHashCode() {
            return this.IdentificationNumber[0].GetHashCode();
        }

        public override bool Equals(Object other) {
            PassportInfo otherPassport = other as PassportInfo;
            if (otherPassport == null) {
                return false;
            }
            return this.IdentificationNumber == otherPassport.IdentificationNumber;
        }
    }
}
