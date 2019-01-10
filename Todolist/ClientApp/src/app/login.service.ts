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

  updateUserAuthenticationStatus() {
    return this.httpClient.get<boolean>('/api/account/isAuthenticated', { withCredentials: true }).pipe(tap(isAuthenticated => {
      this._isUserAuthenticatedSubject.next(isAuthenticated);
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
    //this.httpClient.post('/account/logout').subscribe(_ => {
    //  //redirect the user to a page that does not require authentication
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
