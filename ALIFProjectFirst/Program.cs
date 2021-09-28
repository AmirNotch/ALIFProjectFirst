using System;

namespace ALIFProjectFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        } 
    }
    public class Account
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string SerialNumber { get; set; }
        public int TaxPayerIDNumber { get; set; }
        public int RoleId { get; set; }
    }
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class WorkSheet
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Sex { get; set; }
        public string MaritalStatus { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
    }
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Credit
    {
        public int Id { get; set; }
        public int Account_Id { get; set; }
        public decimal SumOfCredit { get; set; }
        public int CreditHistory { get; set; }
        public string DelaysCreditHistory { get; set; }
        public int CreditTerm { get; set; }
        public int Status_Id { get; set; }
    }
}
