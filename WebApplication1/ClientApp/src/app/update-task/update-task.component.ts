import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { Task } from '../models/task';

@Component({
  selector: 'app-update-task',
  templateUrl: './update-task.component.html',
  styleUrls: ['./update-task.component.css']
})
export class UpdateTaskComponent implements OnInit {

  
  public id: string = "";
  public name: string = "";
  public users: string = "";
  public subtasks: string = "";
  public description: string = "";
  public type: string = "";
  public status: string = "";
  public priority: string = "";
  public date: Date = new Date();
  public project: string = "";

  constructor(private fetcher: FetcherService, private authorizeService: AuthorizeService, private router: Router, private route: ActivatedRoute) { 
  }

  ngOnInit() {
    this.authorizeService.isAuthenticated().subscribe(auth => {
      if (!auth) {
        this.router.navigate(["/authentication/login"]);
      } else {
        if (this.users === "") {
          this.getTask();
        }
      }
    });
  }

  getTask() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.fetcher.getTaskFromApi(this.id).subscribe(resp => {
      this.name = resp.name;
      this.description = resp.description;
      this.type = resp.type;
      this.date = resp.endDate;
      this.priority = resp.priority;
      this.project = resp.project;
      resp.handledBy.forEach(a => {
        this.fetcher.getAccountFromApiById(a).subscribe(resp => {
          if (resp.username !== undefined && !this.users.includes(resp.username)) {
            this.users += resp.username + ";"
          }
        });
      });
      resp.serverSideTaskList.forEach(s => {
        if (s.description !== undefined && s.description !== "" && !this.subtasks.includes(s.description)) {
          this.subtasks += s.description + ";";
        }
      });
    });
  }

  edit() {
    var t = new Task();
    t.id = this.id;
    t.users = this.users;
    t.name = this.name;
    t.description = this.description;
    t.endDate = this.date;
    t.priority = this.priority;
    t.type =this.type;
    t.status =this.status;
    t.subTasks = this.subtasks;
    t.project = this.project;
    this.fetcher.postUpdateTaskToApi(t).subscribe(resp => this.router.navigate(["project", this.project]), error =>console.log(error));
  }

  back() {
    this.router.navigate(["project", this.project]);
  }
}
