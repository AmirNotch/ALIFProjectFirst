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
            bool workingFirstPart = true;

            string login = "";
            int number = 0;
            while (workingFirstPart)
            {
                Console.WriteLine("Для выбора услуг выбирите номер\n");
                Console.WriteLine("1.Регистрация:\n2.Авторизация:\n3.Выйти из программы");
                int.TryParse(Console.ReadLine(), out var choice);
                switch (choice)
                {
                    case 1:
                        CreateAccount(conString);
                        Console.WriteLine("Авторизуйтесь пожалуйста Чтобы Войти в учётную запись");
                        break;
                    case 2:
                    refresh_number:
                        Console.WriteLine("Введите номер Телефона для авторизации");
                        Console.WriteLine("Например: 888800080\n");
                        int.TryParse(Console.ReadLine(), out var numberRead);
                        number = numberRead;
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
                            TimeSpan morning1 = new TimeSpan(4, 0, 0); //4 o'clock
                            TimeSpan morning2 = new TimeSpan(10, 0, 0); //10 o'clock

                            TimeSpan afternoon1 = new TimeSpan(10, 0, 0); //10 o'clock
                            TimeSpan afternoon2 = new TimeSpan(18, 0, 0); //18 o'clock

                            TimeSpan evening1 = new TimeSpan(18, 0, 0); //18 o'clock
                            TimeSpan evening2 = new TimeSpan(22, 0, 0); //22 o'clock
                            
                            TimeSpan now = DateTime.Now.TimeOfDay;
                            if (now >= morning1 && now <= morning2)
                            {
                                Console.WriteLine($"Доброе утро {login}");
                                workingFirstPart = false; 
                            }
                            else if(now >= afternoon1 && now <= afternoon2) 
                            {
                                Console.WriteLine($"Добрый день {login}");
                                workingFirstPart = false;
                            }
                            else if (now >= evening1 && now <= evening2)
                            {
                                Console.WriteLine($"Добрый вечер {login}");
                                workingFirstPart = false;
                            }
                            else
                            {
                                Console.WriteLine($"Доброй ночи {login}");
                                workingFirstPart = false;
                            }
                        }
                        break;
                    case 3:
                        exitNumber++;
                        workingFirstPart = false;
                        break;
                    default:
                        break;
                }
            }

            if (exitNumber == 1)
            {
                return;
            }

            bool workingSecondPart = true;
            while (workingSecondPart)
            {
                Console.WriteLine("Для выбора услуг выбирите номер\n");
                Console.WriteLine("1.Заполнение Анкеты:\n2.Заявка на кредит:\n3.Посмотреть историю заявок:\n4.Выйти из программы");
                int.TryParse(Console.ReadLine(), out var choice);
                switch (choice)
                {
                    case 1:
                        int accountId = GetAccointId(number, conString);
                        int workSheetId = GetWorkSheetAccointId(accountId, conString);
                        if (workSheetId == 0)
                        {
                            CreateWorkSheet(conString, number);
                        }
                        else
                        {
reshenie:
                            Console.WriteLine("У вас уже есть есть анкета\nХотите заполнить новую\n1.Да\n2.Нет");
                            int.TryParse(Console.ReadLine(), out var choiceWorkSheet);
                            switch (choiceWorkSheet)
                            {
                                case 1:
                                    DeleteWorkSheet(conString, accountId);

                                    CreateWorkSheet(conString, number);
                                    break;
                                case 2:
                                    continue;
                                    break;
                                default:
                                    Console.WriteLine("Выберите ваше решение");
                                    goto reshenie;
                                    break;
                            }
                        }
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        exitNumber++;
                        workingSecondPart = false;
                        break;
                    default:
                        break;
                }
            }

            if (exitNumber == 1)
            {
                return;
            }
        }

        private static void DeleteWorkSheet(string conString, int accountId)
        {
            var sqlConnection = new SqlConnection(conString);
            try
            {
                var query = "DELETE * from WorkSheet WHERE Account_Id == @accountId";
                var command = sqlConnection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("@accountId", accountId);
                
                sqlConnection.Open();

                var result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    Console.WriteLine("Вы успешно заполнили Анкету");
                }
                else
                {
                    Console.WriteLine("Что-то пошло не так");
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private static void CreateWorkSheet(string conString, int number)
        {
            var amount = 0;
            Console.WriteLine("Выберите Пол для заполнении анкеты");
            Console.Write("1.Мужской:\n2.Женский:\n");
            var sexString = "";
            var sexNumber = 0;
            while (sexNumber < 1)
            {
                int.TryParse(Console.ReadLine(), out var choice);
                switch (choice)
                {
                    case 1:
                        sexNumber++;
                        amount += 1;
                        sexString = "Муж";
                        break;
                    case 2:
                        sexNumber++;
                        amount += 2;
                        sexString = "Жен";
                        break;
                    default:
                        Console.WriteLine("Пожалуйста выберите пол");
                        break;
                }
            }

            Console.WriteLine("Выберите Семейное Положение");
            Console.Write("1.Холост:\n2.Семьянин:\n3.Вразводе:\n4.Вдовец/Вдова:\n");
            var status = "";
            var statusNumber = 0;
            while (statusNumber < 1)
            {
                int.TryParse(Console.ReadLine(), out var choice);
                switch (choice)
                {
                    case 1:
                        statusNumber++;
                        amount += 1;
                        status = "Холост";
                        break;
                    case 2:
                        statusNumber++;
                        amount += 2;
                        status = "Семьянин";
                        break;
                    case 3:
                        statusNumber++;
                        amount += 1;
                        status = "Вразводе";
                        break;
                    case 4:
                        statusNumber++;
                        amount += 0;
                        status = "Вдовец/Вдова";
                        break;
                    default:
                        Console.WriteLine("Пожалуйста выберите Семейное Положение");
                        break;
                }
            }

            Console.WriteLine("Введите свой возраст Возраст");
            Console.Write("Например 25 лет\n");
            var age = 0;
            var ageNumber = 0;
            while (ageNumber < 1)
            {
                int.TryParse(Console.ReadLine(), out var choice);
                switch (choice)
                {
                    case int n when (n < 16):
                        Console.WriteLine("Извините но до 16 летия нельзя заполнять анкету");
                        Console.WriteLine($"Подождите {16 - n} года");
                        break;
                    case int n when (n <= 25 && n >= 18):
                        ageNumber++;
                        amount += 0;
                        age = choice;
                        break;
                    case int n when (n >= 26 && n <= 35):
                        ageNumber++;
                        amount += 1;
                        age = choice;
                        break;
                    case int n when (n >= 46 && n <= 62):
                        ageNumber++;
                        amount += 2;
                        age = choice;
                        break;
                    case int n when (n >= 63 && n <= 100):
                        ageNumber++;
                        amount += 1;
                        age = choice;
                        break;
                    default:
                        Console.WriteLine("Пожалуйста выберите ваш Возраст");
                        break;
                }
            }
            
            Console.WriteLine("Выберите Ваше гражданство");
            Console.Write("1.Таджикистан:\n2.Зарубеж:\n");
            var nationality = "";
            var nationalityNumber = 0;
            while (nationalityNumber < 1)
            {
                int.TryParse(Console.ReadLine(), out var choice);
                switch (choice)
                {
                    case 1:
                        nationalityNumber++;
                        amount += 1;
                        nationality = "Таджикистан";
                        break;
                    case 2:
                        nationalityNumber++;
                        amount += 0;
                        nationality = "Зарубеж";
                        break;
                    default:
                        Console.WriteLine("Пожалуйста Выберите Ваше гражданство");
                        break;
                }
            }

            int accountId;
            var sqlConnection = new SqlConnection(conString);
            try
            {
                accountId = GetAccointId(number, conString);
                
                WorkSheet workSheet = new WorkSheet
                {
                    Sex = sexString,
                    Age = age,
                    MaritalStatus = status,
                    Nationality = nationality,
                    AccountId = accountId,
                };

                var query = "Insert into WorkSheet(Account_Id, Sex, MaritalStatus, Age, Nationality) Values(@accountId, @sex, @maritalStatus, @age, @nationality)";
                var command = sqlConnection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("@accountId", workSheet.AccountId);
                command.Parameters.AddWithValue("@sex", workSheet.Sex);
                command.Parameters.AddWithValue("@maritalStatus", workSheet.MaritalStatus);
                command.Parameters.AddWithValue("@age", workSheet.Age);
                command.Parameters.AddWithValue("@nationality", workSheet.Nationality);

                sqlConnection.Open();

                var result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    Console.WriteLine("Вы успешно заполнили Анкету");
                }
                else
                {
                    Console.WriteLine("Что-то пошло не так");
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

        }
        private static int GetWorkSheetAccointId(int number, string conString)
        {
            var accNumber = 0;
            var sqlConnection = new SqlConnection(conString);
            var query = "Select Id from WorkSheet where Account_Id = @accountId";

            var command = sqlConnection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue(@"accountId", number);

            sqlConnection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                accNumber = reader.GetInt32(0);
            }

            sqlConnection.Close();
            reader.Close();

            return accNumber;
        }

        private static int GetAccointId(int number, string conString)
        {
            var accNumber = 0;
            var sqlConnection = new SqlConnection(conString);
            var query = "Select Id from Account where PhoneNumber = @phoneNumber";

            var command = sqlConnection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue(@"phoneNumber", number);

            sqlConnection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                accNumber = reader.GetInt32(0);
            }

            sqlConnection.Close();
            reader.Close();

            return accNumber;
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
            SqlConnection sqlConnection = new SqlConnection(conString);
            try
            {
                var account = new Account
                {
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = Convert.ToInt32(phoneNumber),
                    SerialNumber = serialNumber,
                    TaxPayerIDNumber = Convert.ToInt32(taxPayerId),
                    RoleID = 1,
                };


                var query = "Insert into Account(FirstName,LastName,PhoneNumber,SerialNumber,TaxPayerIDNumber,Role_Id) Values(@firstName, @lastName, @phoneNumber, @serialNumber, @taxPayerIDNumber, @roleID)";
                var command = sqlConnection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("@firstName", account.FirstName);
                command.Parameters.AddWithValue("@lastName", account.LastName);
                command.Parameters.AddWithValue("@phoneNumber", account.PhoneNumber);
                command.Parameters.AddWithValue("@serialNumber", account.SerialNumber);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

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
