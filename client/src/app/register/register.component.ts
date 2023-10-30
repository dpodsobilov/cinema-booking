import {Component, Inject, OnInit} from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { UserService } from '../user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
 export class RegisterComponent implements OnInit{
  registerForm = new FormGroup({
    email: new FormControl('', [Validators.email, Validators.maxLength(31), Validators.required]),
    password: new FormControl('', [Validators.maxLength(20), Validators.minLength(6), Validators.required, Validators.pattern('[A-Za-z0-9\_]+')]),
    repeatPassword: new FormControl('', [Validators.maxLength(20), Validators.minLength(6), Validators.required]),
    firstName: new FormControl('', [Validators.maxLength(31), Validators.required, Validators.pattern('[A-Za-zА-Яа-я\-]+')]),
    lastName: new FormControl('', [Validators.maxLength(31), Validators.required, Validators.pattern('[A-Za-zА-Яа-я\-]+')])
  });


  ngOnInit() {
    this.registerForm.get("repeatPassword")?.addValidators([this.passwordsMistmatch()]);
  }

  passwordsMistmatch(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const password = this.registerForm.get("password")?.value;
      const repeatPassword = this.registerForm.get("repeatPassword")?.value;
      if (password != repeatPassword) {
        return { 'passwordsMistmatch': true }
      }
      return null;
    }
  }

  baseUrl = "";

  get email() { return this.registerForm.controls['email']; }
  get password() { return this.registerForm.controls['password']; }
  get repeatPassword() { return this.registerForm.controls['repeatPassword']; }
  get firstName() { return this.registerForm.controls['firstName']; }
  get lastName() { return this.registerForm.controls['lastName']; }

  // constructor(@Inject('BASE_API_URL') baseUrl: string, private userService: UserService) {
  //   this.baseUrl = baseUrl;
  //   this.userService.errors.subscribe(
  //     () => this.registerForm.setErrors({ 'badRequest': true })
  //   );
  // }

  onSubmit(){
    const email = this.registerForm.get('email')?.value!;
    const password = this.registerForm.get('password')?.value!;
    const firstName = this.registerForm.get('firstName')?.value!;
    const lastName = this.registerForm.get('lastName')?.value!;
    //this.userService.register(email, password, firstName, lastName);
  }
}

