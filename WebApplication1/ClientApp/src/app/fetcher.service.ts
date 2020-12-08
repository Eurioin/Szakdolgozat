import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { MongoAccount } from './models/mongo-account';
import { Project } from './models/project';
import { Task } from './models/task';

@Injectable({
  providedIn: 'root'
})
export class FetcherService {

  constructor(private client: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getProjectsFromApi(username: string) {
    return this.client.post<Project[]>(this.baseUrl + 'project/get', { username});
  }

  getAccountFromApi(username: string) {
    return this.client.get<MongoAccount>(this.baseUrl + 'account/user?username=' + username);
  }

  getAccountsFromApi() {
    return this.client.get<MongoAccount[]>(this.baseUrl + 'account/accounts');
  }

  postNewProjectToApi(p: Project) {
    return this.client.post(this.baseUrl + 'project/create', p);
  }


  postUpdateProjectToApi(p: Project) {
    return this.client.post(this.baseUrl + 'project/create', p);
  }

  getProjectFromApi(id: string) {
    return this.client.get<Project>(this.baseUrl + 'project/project?id=' + id);
  }

  getTaskFromApi(id: string) {
    return this.client.get<Task>(this.baseUrl + 'task/get?id=' + id);
  }
}