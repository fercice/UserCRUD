import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Usuario } from '../models/usuario';
import { AddDialogComponent } from '../dialogs/add/add.dialog.component';
import { EditDialogComponent } from '../dialogs/edit/edit.dialog.component';
import { DeleteDialogComponent } from '../dialogs/delete/delete.dialog.component';

@Injectable()
export class DataService {  
  private readonly API_URL = 'http://localhost:49999/api/usuario';    
  private readonly Token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2MTAyMTU4NDQsImV4cCI6MTYxMDMwMjI0NCwiaWF0IjoxNjEwMjE1ODQ0fQ.-_HA8BihjEiq7SBLgu73K7-HoraPCPdrwCUxZKKbR7E'
  private readonly reqHeader = new HttpHeaders({ 
    'Authorization': 'Bearer ' + this.Token
  });  

  dataChange: BehaviorSubject<Usuario[]> = new BehaviorSubject<Usuario[]>([]);  
  dialogData: any;

  constructor (private httpClient: HttpClient,                
               private toastr: ToastrService) {}

  get data(): Usuario[] {
    return this.dataChange.value;
  }

  getDialogData() {
    return this.dialogData;
  }

  /** CRUD METHODS */
  listarUsuarios(): void {
    this.httpClient.get<Usuario[]>(this.API_URL + '/listar', { headers: this.reqHeader }).subscribe(data => {
        this.dataChange.next(data);
      },
      (error: HttpErrorResponse) => {
        console.log (error.name + ' ' + error.message);
        
        if (error.error.errors)
          this.toastr.warning(error.error.errors);
        else
          this.toastr.warning(error.message);
      }
    );
  }

  adicionarUsuario(usuario: Usuario, dialogRef: MatDialogRef<AddDialogComponent>): void {
    this.httpClient.post(this.API_URL, usuario, { headers: this.reqHeader }).subscribe(data => {
        this.dialogData = usuario;
        this.toastr.success('Usuário adicionado com sucesso');
        dialogRef.close(1);
      },
      (error: HttpErrorResponse) => {
        console.log (error.name + ' ' + error.message);
        
        if (error.error.errors)
          error.error.errors.forEach(error => {
            this.toastr.warning(error);
          });          
        else
          this.toastr.warning(error.message);                 
      }
    );
  }

  alterarUsuario(usuario: Usuario, dialogRef: MatDialogRef<EditDialogComponent>): void {
    this.httpClient.put(this.API_URL, usuario, { headers: this.reqHeader }).subscribe(data => {
        this.dialogData = usuario;
        this.toastr.success("Usuário alterado com sucesso");
        dialogRef.close(1);
      },
      (error: HttpErrorResponse) => {
        console.log (error.name + ' ' + error.message);
        
        if (error.error.errors)
          error.error.errors.forEach(error => {
            this.toastr.warning(error);
          }); 
        else
          this.toastr.warning(error.message);        
      }
    );
  }

  excluirUsuario(id: number, dialogRef: MatDialogRef<DeleteDialogComponent>): void {
    this.httpClient.delete(this.API_URL + '/' + id, { headers: this.reqHeader }).subscribe(data => {
      console.log(data['']);
        this.toastr.success("Usuário excluido com sucesso"); 
        dialogRef.close(1); 
      },
      (error: HttpErrorResponse) => {
        console.log (error.name + ' ' + error.message);
        
        if (error.error.errors)
          error.error.errors.forEach(error => {
            this.toastr.warning(error);
          });
        else
          this.toastr.warning(error.message);
      }
    );
  }

  listarEscolaridade() {    
    return this.httpClient.get(this.API_URL + '/escolaridade/listar', { headers: this.reqHeader });
  }
}




