import { Component } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm = new FormGroup({
    email: new FormControl('', [Validators.email, Validators.maxLength(31), Validators.required]),
    password: new FormControl('', [Validators.maxLength(20), Validators.minLength(6), Validators.required, Validators.pattern('[A-Za-z0-9\_]+')]),
  });

  get email() { return this.loginForm.controls['email']; }
  get password() { return this.loginForm.controls['password']; }

  // constructor(private userService: UserService) {
  //   this.userService.errors.subscribe(
  //     () => this.loginForm.setErrors({ 'badLogin': true })
  //   );
  //
  // }

  onSubmit(){
    const email = this.loginForm.get('email')?.value!;
    const password = this.loginForm.get('password')?.value!;

    // this.userService.login(login, password, rememberMe as unknown as boolean);
  }


}
