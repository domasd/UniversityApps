namespace DailyPriceNotifier_Decorator.PriceGetters
{
    public interface IPriceParser
    {
        decimal Parse(string htmlContents);
    }
}
