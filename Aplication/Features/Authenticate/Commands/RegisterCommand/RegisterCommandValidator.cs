﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authenticate.Commands.RegisterCommand
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(p => p.Nombre).NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
           .MaximumLength(80).WithMessage("{PropertyName} no puede exceder los {maxlength} caracteres");

            RuleFor(p => p.Apellido).NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
          .MaximumLength(80).WithMessage("{PropertyName} no puede exceder los {MaxLength} caracteres");

            RuleFor(p => p.Email).NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
            .EmailAddress().WithMessage("{PropertyName} debe ser una direccion valida")
      .MaximumLength(100).WithMessage("{PropertyName} no puede exceder los {MaxLength} caracteres");

            RuleFor(p => p.UserName).NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
      .MaximumLength(10).WithMessage("{PropertyName} no puede exceder los {MaxLength} caracteres");

            RuleFor(p => p.Password).NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
      .MaximumLength(85).WithMessage("{PropertyName} no puede exceder los {MaxLength} caracteres")
      .Matches("^(?=.*[A-Z])(?=.*\\d)(?=.*[^A-Za-z0-9]).{8,}$").WithMessage("la contraseña debe tener un numero, un caracter no alfanumerico y una mayuscula");

            RuleFor(p => p.ConfirmPassword).NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
      .MaximumLength(80).WithMessage("{PropertyName} no puede exceder los {MaxLength} caracteres")
      .Equal(p=>p.Password).WithMessage("{PropertyName} debe ser igual a password")
            .Matches("^(?=.*[A-Z])(?=.*\\d)(?=.*[^A-Za-z0-9]).{8,}$").WithMessage("la contraseña debe tener un numero, un caracter no alfanumerico y una mayuscula");
            ;
        }


    }
}
