import { Injectable } from '@angular/core';
import { AbstractControl, ValidatorFn } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ValidatorService {

  constructor() { }
  specialCharacterValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const regex = /^[a-zA-Z0-9- ]*$/; // Regex allowing letters, numbers,space and hyphen
      const isValid = regex.test(control.value);
      return isValid ? null : { 'invalidCharacters': true };
    };
  }
}
