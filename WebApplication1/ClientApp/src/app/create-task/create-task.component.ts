import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { Task } from '../models/task';

@Component({
  selector: 'app-create-task',
  templateUrl: './create-task.component.html',
  styleUrls: ['./create-task.component.css']
})
export class CreateTaskComponent implements OnInit {

  public name: string = "";
  public users: string = "";
  public subtasks: string = "";
  public description: string = "";
  public type: string = "";
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
        var usr = sessionStorage.getItem('username');
        if (usr !== undefined && !this.users.includes(usr)) {
          this.users += usr + ';';
          this.project = this.route.snapshot.paramMap.get('id');
        }
      }
    });
  }

  create() {
    var t = new Task();
    t.users = this.users;
    t.name = this.name;
    t.description = this.description;
    t.endDate = this.date;
    t.priority = this.priority;
    t.type =this.type;
    t.subTasks = this.subtasks;
    t.project = this.project;
    this.fetcher.postNewTaskToApi(t).subscribe(resp => this.router.navigate(["project",this.project]), error =>console.log(error));
  }
}
