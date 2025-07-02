namespace WaterCarrier.Domain.Models
{
    public class Counterparty
    {
        /// <summary>
        /// Приватный конструктор для создания "пустого" объекта через фабричный метод при ошибке валидации.
        /// Инициализирует поля значениями по умолчанию для предсказуемого и безопасного состояния.
        /// </summary>
        private Counterparty()
        {
            Id = Guid.Empty;
            Name = string.Empty;
            Inn = string.Empty;
            Curator = null!;
        }

        /// <summary>
        /// Уникальный идентификатор сущности.
        /// </summary>
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Inn { get; private set; }
        public Employee Curator { get; private set; }

        /// <summary>
        /// Внутренний конструктор для воссоздания объекта из слоя данных (NHibernate).
        /// </summary>
        internal Counterparty(Guid id, string name, string inn, Employee curator)
        {
            Id = id;
            Name = name;
            Inn = inn;
            Curator = curator;
        }

        /// <summary>
        /// Фабричный метод для создания экземпляра контрагента.
        /// Инкапсулирует логику валидации и гарантирует, что объект не будет создан в невалидном состоянии.
        /// </summary>
        /// <returns>Кортеж, содержащий созданный объект Counterparty и строку с ошибкой (пустую, если всё успешно).</returns>
        public static (Counterparty counterparty, string error) Create(
            Guid id,
            string name,
            string inn,
            Employee curator)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(name))
                error = "Наименование не может быть пустым.";
            else if (string.IsNullOrWhiteSpace(inn))
                error = "ИНН не может быть пустым.";
            else if (!inn.All(char.IsDigit))
                error = "ИНН должен состоять только из цифр.";
            else if (inn.Length != 10 && inn.Length != 12)
                error = "ИНН должен состоять из 10 или 12 цифр.";
            else if (curator == null)
                error = "Необходимо указать куратора.";

            // Тернарный оператор для возврата результата.
            // Если была найдена ошибка, возвращается "пустой" объект Counterparty и текст ошибки.
            // В противном случае создается и возвращается валидный объект Counterparty с новым Guid и пустой строкой ошибки.
            return !string.IsNullOrEmpty(error) ? (new Counterparty(), error) : (new Counterparty
            {
                Id = id == Guid.Empty ? Guid.NewGuid() : id,
                Name = name,
                Inn = inn,
                Curator = curator
            }, string.Empty);
        }
    }
}