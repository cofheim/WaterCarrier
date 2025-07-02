namespace WaterCarrier.Domain.Models
{
    public class Order
    {
        /// <summary>
        /// Приватный конструктор для создания пустого объекта через фабричный метод при ошибке валидации.
        /// Инициализирует поля значениями по умолчанию для предсказуемого и безопасного состояния.
        /// </summary>
        private Order()
        {
            Id = Guid.Empty;
            Date = DateTime.MinValue;
            Amount = 0;
        }

        /// <summary>
        /// Уникальный идентификатор сущности.
        /// </summary>
        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public decimal Amount { get; private set; }
        public Employee Employee { get; private set; }
        public Counterparty Counterparty { get; private set; }

        /// <summary>
        /// Фабричный метод для создания экземпляра заказа.
        /// Инкапсулирует логику валидации и гарантирует, что объект не будет создан в невалидном состоянии.
        /// </summary>
        /// <returns>Кортеж, содержащий созданный объект Order и строку с ошибкой (пустую, если всё успешно).</returns>
        public static (Order order, string error) Create(
            DateTime date,
            decimal amount,
            Employee employee,
            Counterparty counterparty)
        {
            var error = string.Empty;

            if (date > DateTime.UtcNow)
                error = "Дата заказа не может быть в будущем.";
            else if (amount <= 0)
                error = "Сумма должна быть положительным числом.";
            else if (employee == null)
                error = "Необходимо указать сотрудника.";
            else if (counterparty == null)
                error = "Необходимо указать контрагента.";

            // Если была найдена ошибка, возвращается пустой объект Order и текст ошибки.
            // В противном случае создается и возвращается валидный объект Order с новым Guid и пустой строкой ошибки.
            return !string.IsNullOrEmpty(error) ? (new Order(), error) : (new Order
            {
                Id = Guid.NewGuid(),
                Date = date,
                Amount = amount,
                Employee = employee,
                Counterparty = counterparty
            }, string.Empty);
        }
    }
}