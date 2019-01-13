import { Tasklist } from './tasklist';

export class Task {
  constructor(
    public id?: number,
    public description?: string,
    public tasklist?: Tasklist,
    public iscompleted?: boolean) { }
}
