namespace LibraryManagement.API.DTOs.Request;

public class LoanRequestDTO
{
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
}
