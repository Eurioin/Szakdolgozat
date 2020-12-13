import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { Task } from '../models/task';
import { Megjegyzes} from '../models/comment';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent implements OnInit {
  public Task: Task = new Task();
  public Comment: string = "";

  constructor(private fetcher: FetcherService, private authorizeService: AuthorizeService, private router: Router, private route: ActivatedRoute) { 
  }

  ngOnInit() {
    this.authorizeService.isAuthenticated().subscribe(auth => {
      if (!auth) {
        this.router.navigate(["/authentication/login"]);
      } else {
        this.fetcher.getTaskFromApi(this.route.snapshot.paramMap.get('id')).subscribe(resp => {
          this.Task = resp;
        });
      }
    });
  }

  edit() {
    this.router.navigate(["task/update", this.Task.id]);
  }

  back() {
    this.router.navigate(["project", this.Task.project]);
  }

  delete() {
    var proj = this.Task.project;
    this.fetcher.deleteTaskUsingApi(this.Task).subscribe(resp => this.router.navigate(["project", proj]), err => console.log(err));
  }

  comment() {
    var c = new Megjegyzes();
    c.authorId = sessionStorage.getItem('username');
    c.content = this.Comment;
    c.task = this.Task.id;
    this.fetcher.sendComment(c).subscribe(resp => this.router.navigate(['task', this.Task.id]), err => console.log(err));
    this.Comment = "";
    window.location.reload();
  }
}
