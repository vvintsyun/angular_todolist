import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(public snackBar: MatSnackBar) { }

  showError(message: string): void {
    this.snackBar.open(message, null, {
      duration: 2000,
      horizontalPosition: 'end'
    });
  }
}
