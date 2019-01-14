import { Tasklist } from './tasklist';

export class Task {
  constructor(
    public id?: number,
    public description?: string,
    public tasklist?: Tasklist,
    public tasklistid?: number,
    public iscompleted?: boolean) { }
}
