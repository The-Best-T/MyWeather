using FluentValidation;

namespace UseCases.Commands.UpdateLocation;

public class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
{
    public UpdateLocationCommandValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(1)
            .MaximumLength(60);

        RuleFor(x => x.Latitude)
            .GreaterThanOrEqualTo(-90)
            .LessThanOrEqualTo(90);

        RuleFor(x => x.Longitude)
            .GreaterThanOrEqualTo(-180)
            .LessThanOrEqualTo(180);
    }
}