import { HttpClient, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';
import { Injectable, Inject } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';


export function checkIfUserIsAuthenticated(accountService: SecurityService) {
  console.log('checkifuserauth');
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
      console.log(isAuthenticated + ' real');
    }));
  }
}

@Injectable()
export class LoginService {

  constructor(@Inject(DOCUMENT) private document: Document, private httpClient: HttpClient) { }

  login() {
    this.document.location.href = '/api/account/google-login';
  }

  logout() {
    this.document.location.href = '/account/logout';
    //this.httpClient.get('/account/logout').subscribe(_ => {
    //  this.document.location.href = '/';      
    //});
  }
}

//@Injectable()
//export class Interceptor401Service implements HttpInterceptor {

//  constructor(private accountService: SecurityService) { }

//  intercept(req: HttpRequest<any>, next: HttpHandler) {

//    return next.handle(req).pipe(tap(nonErrorEvent => {
//      //nothing to do there
//    }, (error: HttpErrorResponse) => {
//      if (error.status === 401)
//        this.accountService.setUserAsNotAuthenticated();
//    }));
//  }
//}
