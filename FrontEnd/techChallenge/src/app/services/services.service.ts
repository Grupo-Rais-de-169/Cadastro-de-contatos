import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';

import { AuthLoginModel } from '../models/AuthLoginModel';
import { tokenResponseModel } from '../models/tokenResponseModel';

@Injectable({
  providedIn: 'root'
})



export class ServicesService{
  private accessToken = '';
  constructor(
    private httpClient: HttpClient
  ) { }

  getToken() {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + this.accessToken
    });
    return headers;
  }

  public getLogin(requestLogin: AuthLoginModel): Observable<tokenResponseModel> {
    return this.httpClient.post<tokenResponseModel>(
      `${environment.apiURL}/auth/login`,
      requestLogin
    ).pipe(
      tap(
        (loginResponse)=> this.setToken(loginResponse))
    );
  }

  private setToken(response: tokenResponseModel):void{
    sessionStorage.setItem("token",response.token);
    sessionStorage.setItem("refreshToken",response.refreshToken);
  }
}