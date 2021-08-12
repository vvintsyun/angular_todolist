import {Component, OnInit, OnDestroy, Input} from '@angular/core';
import { TaskList } from '../tasklist';
import { TaskListDataService } from '../data.service';
import { Subscription } from 'rxjs';
import { SecurityService, LoginService } from '../login.service';
import {Router} from '@angular/router';

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
              private router: Router,
              private dataService: TaskListDataService) { }

  ngOnInit() {
    this.subscription = this.accountService.isUserAuthenticated$.subscribe(isAuthenticated => {
      this.isUserAuthenticated = isAuthenticated;
      if (this.isUserAuthenticated) {
        this.accountService.getUserName().subscribe(theName => {
          this.userName = theName;
        });
        this.loadTaskLists();
      }
    });
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
    this.dataService.getAllTaskLists()
      .subscribe((data: TaskList[]) => this.taskLists = data);
  }

  save() {
    const name = this.editableTaskList.name;
    if (!name) {
      return;
    }
    if (this.editableTaskList.id == null) {
      this.dataService.createTaskList(this.editableTaskList)
        .subscribe(_ => this.loadTaskLists());
    } else {
      this.dataService.updateTaskList(this.editableTaskList)
        .subscribe(_ => this.loadTaskLists());
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
      .subscribe(_ => {
        if (tl.id === this.currentTaskListId) {
          this.router.navigate(['/home']);
        }
        else {
          this.loadTaskLists();
        }
      });
    return false;
  }

  add() {
    this.cancel();
    this.tableMode = false;
  }
}
