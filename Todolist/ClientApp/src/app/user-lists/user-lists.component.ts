import {Component, OnInit, OnDestroy, Input} from '@angular/core';
import { TaskList } from '../tasklist';
import { TaskListDataService } from '../services/data.service';
import {of, Subscription, zip} from 'rxjs';
import { SecurityService, LoginService } from '../services/login.service';
import {Router} from '@angular/router';
import {catchError, switchMap} from 'rxjs/operators';
import {NotificationService} from '../services/error-notification.service';

@Component({
  selector: 'app-user-lists',
  templateUrl: './user-lists.component.html',
  providers: [TaskListDataService],
  styleUrls: ['./user-lists.component.css']
})
export class UserListsComponent implements OnInit, OnDestroy {
  @Input() currentTaskListId: number;
  editableTaskList: TaskList = new TaskList();
  taskLists: TaskList[];
  tableMode: boolean = true;

  isUserAuthenticated = false;
  subscription: Subscription;
  userName: string;

  constructor(private accountService: SecurityService,
              private loginService: LoginService,
              private notification: NotificationService,
              private router: Router,
              private dataService: TaskListDataService) { }

  ngOnInit() {
    this.subscription = this.accountService.isUserAuthenticated$
      .pipe(
        switchMap(isAuthenticated => {
          this.isUserAuthenticated = isAuthenticated;
          if (this.isUserAuthenticated) {
            return zip(this.accountService.getUserName(), this.loadTaskLists())
              .pipe(catchError(() => {
                this.notification.showError('Error on getting data was occured.');
                return of();
              }));
          }
          return zip(of(), of());
        }))
      .subscribe((data: [string, TaskList[]]) => {
          this.userName = data[0];
          this.taskLists = data[1];
        },
        (_ => {
          this.notification.showError('Error on getting data was occured.');
        }));
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  logout() {
    this.loginService.logout();
  }

  downloadZip() {
    this.dataService.downloadZip();
  }

  loadTaskLists() {
    return this.dataService.getAllTaskLists();
  }

  save() {
    const name = this.editableTaskList.name;
    if (!name) {
      return;
    }
    if (this.editableTaskList.id == null) {
      this.dataService.createTaskList(this.editableTaskList)
        .pipe(catchError(err => {
            this.notification.showError('Error on creating task list was occured.');
            return of();
          }),
          switchMap(_ => this.loadTaskLists())
        )
        .subscribe((data: TaskList[]) => this.taskLists = data,
          (_ => {
            this.notification.showError('Error on getting task lists was occured.');
          }));
    } else {
      this.dataService.updateTaskList(this.editableTaskList)
        .pipe(catchError(err => {
            this.notification.showError('Error on updating task list was occured.');
            return of();
          }),
          switchMap(_ => this.loadTaskLists())
        )
        .subscribe((data: TaskList[]) => this.taskLists = data,
          (_ => {
            this.notification.showError('Error on getting tasks was occured.');
          }));
    }
    this.cancel();
  }

  editTaskList(tl: TaskList) {
    this.editableTaskList = Object.assign({}, tl);
    return false;
  }

  cancel() {
    this.editableTaskList = new TaskList();
    this.tableMode = true;
  }

  delete(event, tl: TaskList) {
    event.stopPropagation();
    this.dataService.deleteTaskList(tl.id)
      .pipe(catchError(err => {
          this.notification.showError('Error on deleting task was occured.');
          return of();
        }),
        switchMap(() => {
          if (tl.id === this.currentTaskListId) {
            this.router.navigate(['/home']);
          } else {
            return this.loadTaskLists();
          }
        }))
      .subscribe((data: TaskList[]) => this.taskLists = data,
        (_ => {
          this.notification.showError('Error on getting task lists was occured.');
        }));
    return false;
  }

  add() {
    this.cancel();
    this.tableMode = false;
  }
}
