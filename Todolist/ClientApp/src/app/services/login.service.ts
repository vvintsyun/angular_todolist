import { HttpClient } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';
import { Injectable, Inject } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import {switchMap, tap} from 'rxjs/operators';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class SecurityService {
  private _isUserAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isUserAuthenticated$: Observable<boolean> = this._isUserAuthenticatedSubject.asObservable();

  constructor(private httpClient: HttpClient) { }

  getUserName() {
    return this.httpClient.get('/api/account/name', { responseType: 'text', withCredentials: true });
  }

  getUserId() {
    return this.httpClient.get('/api/account/userid', { responseType: 'text', withCredentials: true });
  }

  updateUserAuthenticationStatus() {
    return this.getIsAuthenticated().pipe(tap(isAuthenticated => {
      this._isUserAuthenticatedSubject.next(isAuthenticated);
    }));
  }

  getIsAuthenticated() {
    return this.httpClient.get<boolean>('/api/account/isAuthenticated', { withCredentials: true });
  }
}

@Injectable()
export class LoginService {

  constructor(@Inject(DOCUMENT) private document: Document,
              private router: Router,
              private httpClient: HttpClient,
              private accountService: SecurityService) { }

  login() {
    this.document.location.href = '/api/account/google-login';
  }

  logout() {
    this.httpClient.post('/api/account/logout', { withCredentials: true })
      .pipe(switchMap(() => this.accountService.updateUserAuthenticationStatus()))
      .subscribe(_ => this.router.navigate(['/']));
  }
}
