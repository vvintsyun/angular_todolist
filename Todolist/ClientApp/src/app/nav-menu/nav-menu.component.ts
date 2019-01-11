import { Component, OnInit } from '@angular/core';
import { Tasklist } from '../tasklist';
import { TasklistDataService } from '../data.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  providers: [TasklistDataService],
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  tasklist: Tasklist = new Tasklist();
  tasklists: Tasklist[];
  tableMode: boolean = true;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }


  constructor(private dataService: TasklistDataService) { }

  ngOnInit() {
    this.loadTasklists();
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
