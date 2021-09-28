using System;
using System.Data.SqlClient;

namespace ALIFProjectFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            var conString = @"Data Source = .\Dev; Initial Catalog = ALIFPRojectFirst; Integrated Security = true";

            Console.WriteLine("Добрый день Вас приветствует программа Alif Deposit\n");
            
            bool working = true;

            while (working)
            {
                Console.WriteLine("Для выбора услуг выбирите номер\n");
                Console.WriteLine("1.Регистрация:\n2.Авторизация:");
                int.TryParse(Console.ReadLine(), out var choice);
                switch (choice)
                {
                    case 1:
                        
                        break;
                    case 2:
refresh_number:
                        Console.WriteLine("Введите номер Телефона для авторизации");
                        Console.WriteLine("Например: 888800080\n");
                        int.TryParse(Console.ReadLine(), out var number);
                        if (number == 0)
                        {
                            Console.WriteLine("Вы ввели неправильный номер попробуйте заного");
                            goto refresh_number;
                        }
                        int accounNumber = GetPhoneNumber(number, conString);
                        if (accounNumber == 0)
                        {
                            Console.WriteLine("Вы не Зарегистрированы");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        

        private static int GetPhoneNumber(int number, string conString)
        {
            var accPhoneNumber = 0;
            SqlConnection sqlConnection = new SqlConnection(conString);
            var query = "Select PhoneNumber from Account where PhoneNumber = @phonenumber";

            var command = sqlConnection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@phonenumber", number);

            sqlConnection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                accPhoneNumber = reader.GetInt32(0);
            }

            sqlConnection.Close();
            reader.Close();

            return accPhoneNumber;
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
