#pragma warning  disable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IziHardGames.Libs.Tests;

public abstract class TestCRUD<TResultRead, TResultCreate, TResultUpdate, TResultDelete>
{
    public TResultCreate resultCreate;
    public TResultCreate inputToUpdate;
    public TResultCreate inputToDelete;

    public TResultRead resultReadCreated;
    public TResultRead resultReadDeleted;
    public TResultRead resultReadNonExisting;
    public TResultRead resultReadAfterUpdate;

    public TResultDelete resultDelete;
    public TResultDelete resultDeleteNonExisting;

    public TResultUpdate resultUpdate;
    public TResultUpdate resultUpdateNonExisting;

    private readonly int countRepeats;

    public TestCRUD(int countRepeats)
    {
        this.countRepeats = countRepeats;
    }

    public IEnumerable<int> Execute()
    {
        bool isEven = true;
        for (int i = 0; i < countRepeats; i++)
        {
            Itterate(isEven);
            isEven = !isEven;
            yield return i;
        }
    }

    public async Task Itterate(bool deleteCreated)
    {
        // read non existent
        this.resultReadNonExisting = await ReadNonExisting();
        await ValidateNonExistingRead(resultReadNonExisting);

        // update non existent
        this.resultUpdateNonExisting = await UpdateNonExisting();
        await ValidateNonExistingUpdate(resultUpdateNonExisting);

        // delete non existent
        this.resultDeleteNonExisting = await DeleteNonExisting();
        await ValidateNonExistingDelete(resultDeleteNonExisting);

        // create
        this.resultCreate = await Create();
        await ValidateCreate(resultCreate);

        inputToUpdate = await RandomizeToUpdate(CloneUtility.DeepCloneWithDeepCloner<TResultCreate>(resultCreate));
        inputToDelete = CloneUtility.DeepCloneWithDeepCloner<TResultCreate>(resultCreate);

        // read created
        this.resultReadCreated = await ReadCreated();
        await ValidateReadCreated(resultReadCreated);

        // update created
        this.resultUpdate = await UpdateCreated();
        await ValidateUpdateCreated(resultUpdate);
        // read updated
        this.resultReadAfterUpdate = await ReadUpdated();
        await ValidateReadUpdated(resultReadAfterUpdate);

        if (deleteCreated)
        {
            // delete created 
            this.resultDelete = await DeleteCreated();
            await ValidateDeleteCreated(resultDelete);
            // get deleted (Ensure delete)
            this.resultReadDeleted = await ReadDeleted();
            await ValidateNonExistingRead(resultReadDeleted);
        }
    }


    protected abstract Task<TResultCreate> Create();
    protected abstract Task<TResultRead> ReadCreated();
    protected abstract Task<TResultRead> ReadDeleted();
    protected abstract Task<TResultRead> ReadNonExisting();
    protected abstract Task<TResultRead> ReadUpdated();
    protected abstract Task<TResultUpdate> UpdateCreated();
    protected abstract Task<TResultUpdate> UpdateNonExisting();
    protected abstract Task<TResultDelete> DeleteCreated();
    protected abstract Task<TResultDelete> DeleteNonExisting();
    protected abstract Task ValidateNonExistingRead(TResultRead result);
    protected abstract Task ValidateNonExistingUpdate(TResultUpdate result);
    protected abstract Task ValidateNonExistingDelete(TResultDelete result);
    protected abstract Task ValidateCreate(TResultCreate result);
    protected abstract Task ValidateReadCreated(TResultRead result);
    protected abstract Task ValidateUpdateCreated(TResultUpdate result);
    protected abstract Task ValidateReadUpdated(TResultRead result);
    protected abstract Task ValidateDeleteCreated(TResultDelete result);
    protected abstract Task<TResultCreate> RandomizeToUpdate(TResultCreate created);
}