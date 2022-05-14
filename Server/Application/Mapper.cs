using Accounting.Application.Accounts;
using Accounting.Application.Entries;
using Accounting.Application.Verifications;
using Accounting.Domain.Entities;

using static Accounting.Application.Shared;

namespace Accounting.Application;

public static class Mappings
{
    public static EntryDto ToDto(this Entry e)
    {
        return new EntryDto(
                    e.Id,
                    e.Date,
                    new VerificationShort
                    {
                        VerificationNo = e.VerificationNo,
                        Date = e.Verification.Date,
                        Description = e.Verification.Description,
                    },
                    new AccountShortDto
                    {
                        AccountNo = e.AccountNo,
                        Name = e.Account.Name
                    },
                    e.Description,
                    e.Debit,
                    e.Credit
                );
    }

    public static VerificationDto ToDto(this Verification v)
    {
        return new VerificationDto
        {
            VerificationNo = v.VerificationNo,
            Date = v.Date,
            Description = v.Description,
            Debit = v.Entries.Sum(e => e.Debit.GetValueOrDefault()),
            Credit = v.Entries.Sum(e => e.Credit.GetValueOrDefault()),
            Attachments = v.Attachments.Select(e => e.ToDto())
        };
    }

    public static AttachmentDto ToDto(this Attachment a)
    {
        return new AttachmentDto
        {
            Id = a.Id,
            Url = GetAttachmentUrl(a.Id)!
        };
    }
}