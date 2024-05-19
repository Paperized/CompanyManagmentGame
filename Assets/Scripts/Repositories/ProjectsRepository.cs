using Data;
using Repositories.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Repositories
{
    [Serializable]
    public class ProjectsData : IDataRepository
    {
        public List<ProjectData> onGoingProjects;
        public List<ProjectData> potentialProjects;

        public void Reset()
        {
            onGoingProjects = new List<ProjectData>();
            potentialProjects = new List<ProjectData>();
        }
    }

    public class ProjectsRepository : Repository<ProjectsRepository, ProjectsData>, IDataEventEmitter
    {
        public event Action<List<ProjectData>> OnPotentialProjectsChanged;
        public event Action<ProjectData> OnPotentialProjectAccepted;
        public event Action<ProjectData> OnPotentialProjectDeclined;

        public event Action<List<ProjectData>> OnOngoingProjectsChanged;
        public event Action<ProjectData> OnOngoingProjectStarted;

        private bool DoesIdExistsAnywhere(ProjectData projectData)
        {
            return data.onGoingProjects.Contains(projectData) || data.potentialProjects.Contains(projectData);
        }

        /// <summary>
        /// Start a new ongoing project, can also be outside the potential projects
        /// </summary>
        /// <param name="project">project</param>
        /// <returns>true if the action had effect</returns>
        public bool StartNewOngoingProject(ProjectData project)
        {
            if (data.onGoingProjects.Contains(project)) return false;

            data.onGoingProjects.Add(project);
            OnOngoingProjectStarted?.Invoke(project);
            OnOngoingProjectsChanged?.Invoke(data.onGoingProjects);

            return true;
        }

        /// <summary>
        /// Start a new ongoing project, can also be outside the potential projects
        /// </summary>
        /// <param name="project">project</param>
        /// <returns>true if the action had effect</returns>
        public bool AddNewPotentialProject(ProjectData project)
        {
            if (DoesIdExistsAnywhere(project)) return false;

            data.potentialProjects.Add(project);
            OnPotentialProjectsChanged?.Invoke(data.potentialProjects);
            return true;
        }

        /// <summary>
        /// Accept or decline an existing potential project
        /// </summary>
        /// <param name="id">id of potential project</param>
        /// <param name="isAccepted">accepted or declined</param>
        /// <returns>true if the action had effect</returns>
        public bool AcceptOrDeclinePotentialProject(string id, bool isAccepted)
        {
            ProjectData potentialProject = data.potentialProjects.Find(x => id.Equals(x.id));

            if(potentialProject is null) return false;

            data.potentialProjects.Remove(potentialProject);

            if(isAccepted)
            {
                bool wasAdded = StartNewOngoingProject(potentialProject);
                if(wasAdded)
                {
                    OnPotentialProjectAccepted?.Invoke(potentialProject);
                }
            } 
            else
            {
                OnPotentialProjectDeclined?.Invoke(potentialProject);
            }

            OnPotentialProjectsChanged?.Invoke(data.potentialProjects);
            return true;
        }

        public void BroadcastAllData()
        {
            OnPotentialProjectsChanged?.Invoke(data.potentialProjects);
            OnOngoingProjectsChanged?.Invoke(data.onGoingProjects);
        }

        public override string FileName() => "projects";
    }
}