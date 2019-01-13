import { Component, OnInit, OnDestroy } from '@angular/core';
import { Tasklist } from '../tasklist';
import { TasklistDataService } from '../data.service';
import { Subscription } from 'rxjs';
import { SecurityService, LoginService } from '../login.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  providers: [TasklistDataService],
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit, OnDestroy {
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
    console.log(this.accountService.isUserAuthenticated);
    this.subscription = this.accountService.isUserAuthenticated.subscribe(isAuthenticated => {
      console.log(isAuthenticated + ' got');
      this.isUserAuthenticated = isAuthenticated;
      if (this.isUserAuthenticated) {
        this.accountService.getUserName().subscribe(theName => {
          this.userName = theName;
        });
      }
      console.log(this.isUserAuthenticated);
    });
    this.loadTasklists();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  logout() {
    this.loginService.logout();
  }

  loadTasklists() {
    this.dataService.getTasklists()
      .subscribe((data: Tasklist[]) => this.tasklists = data);    
  }
  
  save() {
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
  delete(tl: Tasklist) {
    this.dataService.deleteTasklist(tl.id)
      .subscribe(data => this.loadTasklists());
  }
  add() {
    //this.cancel();
    this.tableMode = false;
  }  
}
