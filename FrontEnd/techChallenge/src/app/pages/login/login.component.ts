import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthLoginModel } from '../../models/AuthLoginModel';
import { ServicesService } from '../../services/services.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { tokenResponseModel } from '../../models/tokenResponseModel';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit{
  responsetoken!: tokenResponseModel;
  public requestAuth! : AuthLoginModel
  formAuth = new FormGroup({
    login: new FormControl<string>(''),
    password: new FormControl<string>('')
  });
  constructor(private service: ServicesService){}

  ngOnInit(): void {
    
  }

  async OnSubmit(){
    console.log(this.formAuth.value);
    this.requestAuth = {
      login: this.formAuth.controls.login.value!, 
      password: this.formAuth.controls.password.value!
    };

    this.service.getLogin(this.requestAuth).subscribe({
      next: (data) => {
        this.responsetoken = data
      },
      error: (error) =>{
        console.error("Erro ao solicitar Token", error);
      },
      complete:()=>{
        //localStorage.setItem("token",this.responsetoken.token);
        //localStorage.setItem("refreshToken",this.responsetoken.refreshToken);
        console.log(this.responsetoken.token);
      }
    })
    
  }
}
