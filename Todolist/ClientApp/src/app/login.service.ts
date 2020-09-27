import { HttpClient, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';
import { Injectable, Inject } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';


export function checkIfUserIsAuthenticated(accountService: SecurityService) {
  return () => accountService.updateUserAuthenticationStatus().toPromise();
}

@Injectable({
  providedIn: 'root'
})
export class SecurityService {
  private _isUserAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isUserAuthenticated: Observable<boolean> = this._isUserAuthenticatedSubject.asObservable();

  constructor(@Inject(DOCUMENT) private document: Document, private httpClient: HttpClient) { }

  getUserName() {
    return this.httpClient.get('/api/account/name', { responseType: 'text', withCredentials: true });
  }

  getUserId() {
    return this.httpClient.get('/api/account/userid', { responseType: 'text', withCredentials: true });
  }

  updateUserAuthenticationStatus() {
    return this.httpClient.get<boolean>('/api/account/isAuthenticated', { withCredentials: true }).pipe(tap(isAuthenticated => {
      this._isUserAuthenticatedSubject.next(isAuthenticated);
    }));
  }
}

@Injectable()
export class LoginService {

  constructor(@Inject(DOCUMENT) private document: Document, private httpClient: HttpClient, private accountService: SecurityService) { }

  login() {
    this.document.location.href = '/api/account/google-login';
    this.accountService.updateUserAuthenticationStatus().subscribe();
  }

  logout() {
    this.httpClient.post('/api/account/logout', { withCredentials: true }).subscribe(_ => {
      this.accountService.updateUserAuthenticationStatus().subscribe();
    });
    this.document.location.href = '/';
  }
}
