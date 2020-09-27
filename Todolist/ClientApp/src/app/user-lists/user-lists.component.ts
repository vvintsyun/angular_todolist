import { Component, OnInit, OnDestroy } from '@angular/core';
import { Tasklist } from '../tasklist';
import { TasklistDataService } from '../data.service';
import { Subscription } from 'rxjs';
import { SecurityService, LoginService } from '../login.service';

@Component({
  selector: 'app-user-lists',
  templateUrl: './user-lists.component.html',
  providers: [TasklistDataService],
  styleUrls: ['./user-lists.component.css']
})
export class UserListsComponent implements OnInit, OnDestroy {
  isExpanded = false;
  tasklist: Tasklist = new Tasklist();
  tasklists: Tasklist[];
  tableMode: boolean = true;

  isUserAuthenticated = false;
  subscription: Subscription;
  userName: string;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  constructor(private accountService: SecurityService, private loginService: LoginService, private dataService: TasklistDataService) { }

  ngOnInit() {
    this.subscription = this.accountService.isUserAuthenticated.subscribe(isAuthenticated => {
      this.isUserAuthenticated = isAuthenticated;
      if (this.isUserAuthenticated) {
        this.accountService.getUserName().subscribe(theName => {
          this.userName = theName;
        });
        this.loadTasklists();
      }
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  logout() {
    this.loginService.logout();
  }

  downloadzip() {
    this.dataService.downloadlists();
  }

  loadTasklists() {
    this.dataService.getAllTasklists()
      .subscribe((data: Tasklist[]) => this.tasklists = data);
  }

  save() {
    const name = this.tasklist.name;
    if (this.tasklist.id == null) {
      this.dataService.createTasklist(this.tasklist)
        .subscribe((data: Tasklist) => this.tasklists.push(data));
    } else {
      this.dataService.updateTasklist(this.tasklist)
        .subscribe(data => this.loadTasklists());
    }
    this.cancel();
  }

  editTasklist(tl: Tasklist) {
    this.tasklist = tl;
    return false;
  }

  cancel() {
    this.tasklist = new Tasklist();
    this.tableMode = true;
  }

  delete(event, tl: Tasklist) {
    event.stopPropagation();
    this.dataService.deleteTasklist(tl.id)
      .subscribe(data => this.loadTasklists());
    return false;
  }

  add() {
    this.cancel();
    this.tableMode = false;
  }
}
