import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { MongoAccount } from '../models/mongo-account';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  public Account: MongoAccount = new MongoAccount();
  public name: string;
  public phoneNumber: string;
  public email: string;

  constructor(private fetcher: FetcherService, private authorizeService: AuthorizeService, private router: Router) { 
  }

  ngOnInit() {
    var i = 0;
    this.authorizeService.isAuthenticated().subscribe(auth => {
      if (i++ === 0) {
        if (!auth) {
          this.router.navigate(["/authentication/login"]);
        } else {
          this.getAccountByUsername(sessionStorage.getItem('username'));
        }
      }
    });
  }

  getAccountByUsername(username: string) {
    this.fetcher.getAccountFromApi(username).subscribe(resp => {
      if (!this.Account.username) {
        this.Account=resp;
        this.email = this.Account.email;
        this.phoneNumber = this.Account.phoneNumber;
        this.name = this.Account.name;
      }
    }, error => console.log(error));
  }

  edit() {
    this.Account.phoneNumber = this.phoneNumber;
    this.Account.email = this.email;
    this.Account.name = this.name;
    this.fetcher.postUpdateAccountToApi(this.Account).subscribe(resp => this.Account = resp, err => console.log(err))
  }
}
