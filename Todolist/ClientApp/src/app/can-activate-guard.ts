import {Inject, Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot} from '@angular/router';
import {SecurityService} from './services/login.service';
import {Observable} from 'rxjs';

@Injectable()
export class AuthGuard
  implements CanActivate {
  constructor(
    @Inject(SecurityService) private securityService: SecurityService
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.securityService.getIsAuthenticated();
  }
}
