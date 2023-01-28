namespace FriMav.Application
{
    public class EmployeeResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public int Absences { get; set; }
        public decimal Balance { get; set; }
        public bool IsLiquidated { get; set; }
        public decimal LiquidationBalance { get; set; }
        public bool IsPenalized { get; set; }
    }
}