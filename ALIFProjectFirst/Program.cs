using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace ALIFProjectFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            var conString = @"Data Source = .\Dev; Initial Catalog = ALIFProjectFirst; Integrated Security = true";

            Console.WriteLine("Добрый день Вас приветствует программа Alif Deposit\n");

            var exitNumber = 0;
            bool working = true;

            while (working)
            {
                Console.WriteLine("Для выбора услуг выбирите номер\n");
                Console.WriteLine("1.Регистрация:\n2.Авторизация:\n3.Выйти из программы");
                int.TryParse(Console.ReadLine(), out var choice);
                switch (choice)
                {
                    case 1:
                        CreateAccount(conString);
                        break;
                    case 2:
refresh_number:
                        string login = "";
                        Console.WriteLine("Введите номер Телефона для авторизации");
                        Console.WriteLine("Например: 888800080\n");
                        int.TryParse(Console.ReadLine(), out var number);
                        if (number == 0)
                        {
                            Console.WriteLine("Вы ввели неправильный номер попробуйте заного");
                            goto refresh_number;
                        }
                        int accounNumber = GetPhoneNumber(number, conString, ref login);
                        if (accounNumber == 0)
                        {
                            Console.WriteLine("Вы не Зарегистрированы");
                        }
                        else
                        {
                            /*var datatime = DateTime.Now;
                            if (true)
                            {

                            }*/
                            Console.WriteLine($"Добрый день {login}");
                        }
                        break;

                    case 3:
                        exitNumber++;
                        working = false;
                        break;
                    default:
                        break;
                }
            }

            if (exitNumber == 1)
            {
                return;
            }

            bool working2 = true;
            while (working2)
            {
                
            }
        }

        private static void CreateAccount(string conString)
        {
            Console.WriteLine("Введите своё Имя");
firstName_error:
            var firstName = Console.ReadLine();
            /*var errorCounterFirstName = Regex.IsMatch(firstName, @"[a-zA-Z]|[а-яА-Я]");*/
            bool resultFirstName = firstName.All(Char.IsLetter);
            if (!resultFirstName)
            {
                Console.WriteLine("Имя должно состоять из Букв и без лишних символов и цифр");
                Console.WriteLine("Введите Имя ещё раз");
                goto firstName_error;
            }
            if (string.IsNullOrEmpty(firstName))
            {
                Console.WriteLine("Введите Имя ещё раз");
                goto firstName_error;
            }

            Console.WriteLine("Введите свою Фамилию");
lastName_error:
            var lastName = Console.ReadLine();
            /*var errorCounterLastName = Regex.Matches(lastName, @"[a-zA-Z]").Count;*/
            bool resultLastName = lastName.All(Char.IsLetter);
            if (!resultLastName)
            {
                Console.WriteLine("Фамилие должно состоять из Букв и без лишних символов и цифр");
                Console.WriteLine("Введите Фамилию ещё раз");
                goto lastName_error;
            }
            if (string.IsNullOrEmpty(lastName))
            {
                Console.WriteLine("Введите Фамилию ещё раз");
                goto lastName_error;
            }

            Console.WriteLine("Введите свой Номер Телефона");
        numbers_error:
            //long.TryParse(Console.ReadLine(), out var phoneNumber);
            var phoneNumber = Console.ReadLine();
            //string phoneNumberConvert = phoneNumber.ToString();
            var errorPhoneNumber = Regex.IsMatch(phoneNumber, "^[0-9]{9}$");
            if (!errorPhoneNumber)
            {
                Console.WriteLine("Номер телефона должен содержать только девять цифр");
                Console.WriteLine("Введите пожалуйста номер ещё раз\n");
                goto numbers_error;
            }

            Console.WriteLine("Введите свой Серийный Номер паспорта");
serialNumber_error: 
            var serialNumber = Console.ReadLine();
            
            var errorSerialNumber = Regex.IsMatch(serialNumber, "^(([A-Z]){1})[0-9]{8}$");

            if (!errorSerialNumber)
            {
                Console.WriteLine("Введите правильный серийный номер\nНапример: A01234567");
                goto serialNumber_error;
            }

            Console.WriteLine("Введите свой ИИН");
        numbersPayer_error:
            //int.TryParse(Console.ReadLine(), out var taxPayerIDNumber);
            var taxPayerId = Console.ReadLine();
            var errortaxPayerIDNumber = Regex.IsMatch(taxPayerId, "^[0-9]{9}$");
            if (!errortaxPayerIDNumber)
            {
                Console.WriteLine("Номер телефона должен содержать только девять цифр");
                Console.WriteLine("Введите пожалуйста номер ещё раз\n");
                goto numbersPayer_error;
            }

            /*if (taxPayerIDNumber.ToString().Length == 9)
        {
            Console.WriteLine("ИИН должен содержать 9 цифр");
            Console.WriteLine("Введите ИИН ещё раз");
            goto numbersPayer_error;
        }
        if (taxPayerIDNumber == 0)
        {
            Console.WriteLine("Введите пожалуйста только цифры ещё раз\n");
            goto numbersPayer_error;
        }*/

            var account = new Account
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = Convert.ToInt32(phoneNumber),
                SerialNumber = serialNumber,
                TaxPayerIDNumber = Convert.ToInt32(taxPayerId),
                RoleID = 1,
            };

            SqlConnection sqlConnection = new SqlConnection(conString);
            var query = "Insert into Account(FirstName,LastName,PhoneNumber,SerialNumber,TaxPayerIDNumber,Role_Id) Values(@firstName, @lastName, @phoneNumber, @serialNumber, @taxPayerIDNumber, @roleID)";
            var command = sqlConnection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@firstName",account.FirstName);
            command.Parameters.AddWithValue("@lastName",account.LastName);
            command.Parameters.AddWithValue("@phoneNumber",account.PhoneNumber);
            command.Parameters.AddWithValue("@serialNumber",account.SerialNumber);
            command.Parameters.AddWithValue("@taxPayerIDNumber", account.TaxPayerIDNumber);
            command.Parameters.AddWithValue("@roleID", account.RoleID);

            sqlConnection.Open();

            var result = command.ExecuteNonQuery();

            if (result > 0)
            {
                Console.WriteLine("Вы успешно зарегистрировались");
            }
            else
            {
                Console.WriteLine("Что-то пошло не так");
            }
            sqlConnection.Close();
        }

        private static int GetPhoneNumber(int number, string conString, ref string login)
        {
            var accPhoneNumber = 0;
            SqlConnection sqlConnection = new SqlConnection(conString);
            var query = "Select PhoneNumber, FirstName from Account where PhoneNumber = @phonenumber";

            var command = sqlConnection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@phonenumber", number);

            sqlConnection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                accPhoneNumber = reader.GetInt32(0);
                login = (string)reader.GetValue(1);
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
        public int RoleID { get; set; }
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
