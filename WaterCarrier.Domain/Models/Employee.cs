using WaterCarrier.Domain.Enums;

namespace WaterCarrier.Domain.Models
{
    public class Employee
    {
        /// <summary>
        /// Приватный конструктор для создания пустого объекта через фабричный метод при ошибке валидации.
        /// Инициализирует поля значениями по умолчанию для предсказуемого и безопасного состояния.
        /// </summary>
        private Employee()
        {
            Id = Guid.Empty;
            LastName = string.Empty;
            FirstName = string.Empty;
            Patronymic = string.Empty;
            BirthDate = DateTime.MinValue;
        }

        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Patronymic { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Position Position { get; private set; }

        /// <summary>
        /// Внутренний конструктор для воссоздания объекта из слоя данных (NHibernate).
        /// </summary>
        internal Employee(Guid id, string lastName, string firstName, string patronymic, DateTime birthDate, Position position)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            Patronymic = patronymic;
            BirthDate = birthDate;
            Position = position;
        }

        /// <summary>
        /// Фабричный метод для создания экземпляра сотрудника.
        /// Инкапсулирует логику валидации и гарантирует, что объект не будет создан в невалидном состоянии.
        /// </summary>
        /// <returns>Кортеж, содержащий созданный объект Employee и строку с ошибкой (пустую, если всё успешно).</returns>
        public static (Employee employee, string error) Create(
            Guid id,
            string lastName,
            string firstName,
            string patronymic,
            DateTime birthDate,
            Position position)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(lastName))
                error = "Фамилия не может быть пустой.";
            else if (string.IsNullOrWhiteSpace(firstName))
                error = "Имя не может быть пустым.";
            else if (birthDate > DateTime.UtcNow)
                error = "Дата рождения не может быть в будущем.";

            // Если была найдена ошибка, возвращается пустой объект Employee и текст ошибки.
            // В противном случае создается и возвращается валидный объект Employee с новым Guid и пустой строкой ошибки.
            return !string.IsNullOrEmpty(error) ? (new Employee(), error) : (new Employee
            {
                Id = id == Guid.Empty ? Guid.NewGuid() : id,
                LastName = lastName,
                FirstName = firstName,
                Patronymic = patronymic,
                BirthDate = birthDate,
                Position = position
            }, string.Empty);
        }
    }
}