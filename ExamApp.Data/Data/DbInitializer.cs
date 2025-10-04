using ExamApp.Core.Models;
using System.Security.Cryptography;
using System.Text;

namespace ExamApp.Data.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Test sorularını ekle
            if (!context.Questions.Any())
            {
                context.Questions.AddRange(
                    new Question
                    {
                        Text = "ASP.NET Core hangi programlama dilini kullanır?",
                        OptionA = "Java",
                        OptionB = "C#",
                        OptionC = "Python",
                        OptionD = "JavaScript",
                        CorrectAnswer = "C#",
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Question
                    {
                        Text = "Entity Framework nedir?",
                        OptionA = "Bir veritabanı",
                        OptionB = "Bir ORM aracı",
                        OptionC = "Bir programlama dili",
                        OptionD = "Bir web framework'ü",
                        CorrectAnswer = "Bir ORM aracı",
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Question
                    {
                        Text = "HTML'nin açılımı nedir?",
                        OptionA = "Hyper Text Markup Language",
                        OptionB = "High Tech Modern Language",
                        OptionC = "Hyper Transfer Markup Language",
                        OptionD = "High Text Modern Language",
                        CorrectAnswer = "Hyper Text Markup Language",
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Question
                    {
                        Text = "CSS ne işe yarar?",
                        OptionA = "Veritabanı yönetimi",
                        OptionB = "Sunucu programlama",
                        OptionC = "Web sayfalarını stilize etme",
                        OptionD = "Veri analizi",
                        CorrectAnswer = "Web sayfalarını stilize etme",
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Question
                    {
                        Text = "JavaScript hangi tür bir dildir?",
                        OptionA = "Derlenmiş dil",
                        OptionB = "İşaretleme dili",
                        OptionC = "Scripting dili",
                        OptionD = "Veritabanı dili",
                        CorrectAnswer = "Scripting dili",
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Question
                    {
                        Text = "Türkiye'nin başkenti neresidir?",
                        OptionA = "İstanbul",
                        OptionB = "Ankara", 
                        OptionC = "İzmir",
                        OptionD = "Bursa",
                        CorrectAnswer = "Ankara", 
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Question
                    {
                        Text = "Hangi gezegen Güneş Sistemi'ndeki en büyük gezegendir?",
                        OptionA = "Mars",
                        OptionB = "Venüs",
                        OptionC = "Jüpiter", 
                        OptionD = "Satürn",
                        CorrectAnswer = "Jüpiter",
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Question
                    {
                        Text = "İnsan vücudunda kaç kromozom bulunur?",
                        OptionA = "23",
                        OptionB = "46", 
                        OptionC = "64",
                        OptionD = "32",
                        CorrectAnswer = "46", 
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Question
                    {
                        Text = "Hangi elementin kimyasal sembolü 'O'dur?",
                        OptionA = "Altın",
                        OptionB = "Gümüş",
                        OptionC = "Oksijen", 
                        OptionD = "Osmiyum",
                        CorrectAnswer = "Oksijen", 
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Question
                    {
                        Text = "Mona Lisa tablosu kime aittir?",
                        OptionA = "Van Gogh",
                        OptionB = "Picasso",
                        OptionC = "Michelangelo",
                        OptionD = "Leonardo da Vinci",
                        CorrectAnswer = "Leonardo da Vinci", 
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Question
                    {
                        Text = "Hangi dil programlama dilidir?",
                        OptionA = "HTML",
                        OptionB = "CSS",
                        OptionC = "C#", 
                        OptionD = "XML",
                        CorrectAnswer = "C#", 
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Question
                    {
                        Text = "Dünyanın en uzun nehri hangisidir?",
                        OptionA = "Amazon",
                        OptionB = "Nil", 
                        OptionC = "Mississippi",
                        OptionD = "Yangtze",
                        CorrectAnswer = "Nil", 
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Question
                    {
                        Text = "Hangi meyve turunçgil değildir?",
                        OptionA = "Portakal",
                        OptionB = "Limon",
                        OptionC = "Mandalina",
                        OptionD = "Elma", 
                        CorrectAnswer = "Elma", 
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Question
                    {
                        Text = "İstanbul hangi iki kıtayı birbirine bağlar?",
                        OptionA = "Asya ve Afrika",
                        OptionB = "Avrupa ve Asya", 
                        OptionC = "Avrupa ve Amerika",
                        OptionD = "Asya ve Avustralya",
                        CorrectAnswer = "Avrupa ve Asya", 
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    }, 
                    new Question
                    {
                        Text = "Hangi hayvan memeli değildir?",
                        OptionA = "Yunus",
                        OptionB = "Yarasa",
                        OptionC = "Tavuk", 
                        OptionD = "Balina",
                        CorrectAnswer = "Tavuk",
                        Points = 10,
                        CreatedAt = DateTime.UtcNow
                    }

                );
            }

            // Default admin kullanıcısını ekle (şifre: "admin123")
            if (!context.Users.Any(u => u.Username == "admin"))
            {
                var adminPassword = HashPassword("admin123");
                context.Users.Add(new User
                {
                    Username = "admin",
                    Email = "admin@examapp.com",
                    PasswordHash = adminPassword,
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow
                });
            }

            // Test kullanıcısı ekle (şifre: "123456")
            if (!context.Users.Any(u => u.Username == "testuser"))
            {
                var testPassword = HashPassword("123456");
                context.Users.Add(new User
                {
                    Username = "testuser",
                    Email = "test@example.com",
                    PasswordHash = testPassword,
                    Role = UserRole.User,
                    CreatedAt = DateTime.UtcNow
                });
            }

            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}