
namespace DailyPriceNotifier_Decorator.PriceGetters
{
    public interface IPriceGetter
    {
        decimal PriceToCheck { get; set; }

        void CheckPrice();

        string FormMessage();
    }
}
