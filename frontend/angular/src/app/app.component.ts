import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {DataService} from './services/data.service';
import {HttpClient} from '@angular/common/http';
import {MatDialog} from '@angular/material/dialog';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {Usuario} from './models/usuario';
import {DataSource} from '@angular/cdk/collections';
import {AddDialogComponent} from './dialogs/add/add.dialog.component';
import {EditDialogComponent} from './dialogs/edit/edit.dialog.component';
import {DeleteDialogComponent} from './dialogs/delete/delete.dialog.component';
import {BehaviorSubject, fromEvent, merge, Observable} from 'rxjs';
import {map} from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {  
  displayedColumns = ['id', 'nome', 'sobrenome', 'email', 'dataNascimento', 'escolaridade', 'actions'];
  usuarioDataService: DataService | null;
  dataSource: UsuarioDataSource | null;
  index: number;
  id: number;  

  constructor(public httpClient: HttpClient,
              public dataService: DataService,
              public dialog: MatDialog,              
              public toastr: ToastrService) {}

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  @ViewChild('filter',  {static: true}) filter: ElementRef;

  ngOnInit() {
    this.loadData();
  }

  refresh() {
    this.filter.nativeElement.value = "";
    this.loadData();    
  }

  addNew() {
    const dialogRef = this.dialog.open(AddDialogComponent, {
      data: {usuario: Usuario }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 1) {
        // After dialog is closed we're doing frontend updates
        // For add we're just pushing a new row inside DataService
        this.usuarioDataService.dataChange.value.push(this.dataService.getDialogData());
        this.refreshTable();
        this.refresh();
      }
    });
  }

  startEdit(i: number, id: number, nome: string, sobrenome: string, email: string, dataNascimento: string, escolaridade: string) {
    this.id = id;
    // index row is used just for debugging proposes and can be removed
    this.index = i;
    console.log(this.index);
    const dialogRef = this.dialog.open(EditDialogComponent, {
      data: {id: id, nome: nome, sobrenome: sobrenome, email: email, dataNascimento: dataNascimento, escolaridade: escolaridade}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 1) {
        // When using an edit things are little different, firstly we find record inside DataService by id
        const foundIndex = this.usuarioDataService.dataChange.value.findIndex(x => x.id === this.id);
        // Then you update that record using data from dialogData (values you enetered)
        this.usuarioDataService.dataChange.value[foundIndex] = this.dataService.getDialogData();
        // And lastly refresh table
        this.refreshTable();
        this.refresh();
      }
    });
  }

  deleteItem(i: number, id: number, nome: string, sobrenome: string, email: string, dataNascimento: string, escolaridade: string) {
    this.index = i;
    this.id = id;
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {id: id, nome: nome, sobrenome: sobrenome, email: email, dataNascimento: dataNascimento, escolaridade: escolaridade}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 1) {
        const foundIndex = this.usuarioDataService.dataChange.value.findIndex(x => x.id === this.id);
        // for delete we use splice in order to remove single object from DataService
        this.usuarioDataService.dataChange.value.splice(foundIndex, 1);
        this.refreshTable();
        this.refresh();
      }
    });
  }

  private refreshTable() {
    // Refreshing table using paginator
    // Thanks yeager-j for tips  
    this.paginator._changePageSize(this.paginator.pageSize);
  }

  public loadData() {
    this.usuarioDataService = new DataService(this.httpClient, this.toastr);
    this.dataSource = new UsuarioDataSource(this.usuarioDataService, this.paginator, this.sort);
    fromEvent(this.filter.nativeElement, 'keyup')
      .subscribe(() => {
        if (!this.dataSource) {
          return;
        }
        this.dataSource.filter = this.filter.nativeElement.value;
      });
  }
}

export class UsuarioDataSource extends DataSource<Usuario> {
  _filterChange = new BehaviorSubject('');

  get filter(): string {
    return this._filterChange.value;
  }

  set filter(filter: string) {
    this._filterChange.next(filter);
  }

  filteredData: Usuario[] = [];
  renderedData: Usuario[] = [];

  constructor(public _usuarioDataService: DataService,
              public _paginator: MatPaginator,
              public _sort: MatSort) {
    super();
    // Reset to the first page when the user changes the filter.
    this._filterChange.subscribe(() => this._paginator.pageIndex = 0);
  }

  /** Connect function called by the table to retrieve one stream containing the data to render. */
  connect(): Observable<Usuario[]> {
    // Listen for any changes in the base data, sorting, filtering, or pagination
    const displayDataChanges = [
      this._usuarioDataService.dataChange,
      this._sort.sortChange,
      this._filterChange,
      this._paginator.page
    ];

    this._usuarioDataService.listarUsuarios();

    return merge(...displayDataChanges).pipe(map( () => {
        // Filter data
        this.filteredData = this._usuarioDataService.data.slice().filter((usuario: Usuario) => {
          const searchStr = (usuario.id + 
                             usuario.nome + 
                             usuario.sobrenome + 
                             usuario.email + 
                             usuario. dataNascimento + 
                             usuario.escolaridade + 
                             usuario.escolaridadeNome).toLowerCase();

          return searchStr.indexOf(this.filter.toLowerCase()) !== -1;
        });

        // Sort filtered data
        const sortedData = this.sortData(this.filteredData.slice());

        // Grab the page's slice of the filtered sorted data.
        const startIndex = this._paginator.pageIndex * this._paginator.pageSize;
        this.renderedData = sortedData.splice(startIndex, this._paginator.pageSize);
        return this.renderedData;
      }
    ));
  }

  disconnect() {}

  /** Returns a sorted copy of the database data. */
  sortData(data: Usuario[]): Usuario[] {
    if (!this._sort.active || this._sort.direction === '') {
      return data;
    }

    return data.sort((a, b) => {
      let propertyA: number | string = '';
      let propertyB: number | string = '';

      switch (this._sort.active) {
        case 'id': [propertyA, propertyB] = [a.id, b.id]; break;
        case 'nome': [propertyA, propertyB] = [a.nome, b.nome]; break;
        case 'sobrenome': [propertyA, propertyB] = [a.sobrenome, b.sobrenome]; break;
        case 'email': [propertyA, propertyB] = [a.email, b.email]; break;
        case 'dataNascimento': [propertyA, propertyB] = [a.dataNascimento, b.dataNascimento]; break;
        case 'escolaridade': [propertyA, propertyB] = [a.escolaridadeNome, b.escolaridadeNome]; break;
      }

      const valueA = isNaN(+propertyA) ? propertyA : +propertyA;
      const valueB = isNaN(+propertyB) ? propertyB : +propertyB;

      return (valueA < valueB ? -1 : 1) * (this._sort.direction === 'asc' ? 1 : -1);
    });
  }
}
