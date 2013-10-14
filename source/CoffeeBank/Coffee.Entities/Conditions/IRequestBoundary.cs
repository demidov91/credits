namespace Coffee.Entities
{
    public interface IRequestBoundary
    {
        bool IsAcceptable(CreditRequest request);

        string Visualize();
    }
}
