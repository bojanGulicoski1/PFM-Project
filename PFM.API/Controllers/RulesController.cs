﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PFM.API.Entities;
using PFM.API.Interfaces;
using PFM.API.Models;

namespace PFM.API.Controllers
{
    [ApiController]
    [Route("api/rules")]
    public class RulesController : ControllerBase
    {
        private readonly IRuleRepository _ruleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public RulesController(IRuleRepository ruleRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _ruleRepository = ruleRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutoCategorizeRule>>> GetRules(string? catCode)
        {
            var rules = await _ruleRepository.GetAll(catCode);

            return Ok(_mapper.Map<IEnumerable<AutoCategorizeRule>>(rules));
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddRule(AutoCategorizeRule rule)
        {
            var databaseRule = _mapper.Map<Rule>(rule);

            await _ruleRepository.AddRule(databaseRule);

            return Ok(new
            {
               Message = $"Sucesfully instered new rule,with ID: {databaseRule.Id}"  
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteRule(int id)
        {
            var rule = await _ruleRepository.GetByid(id);
            if (rule == null)
            {
                return BadRequest(new
                {
                    Description = "Problem with rules",
                    Message = "Rule does not exist.",
                    StatusCode = 404,

                });
            }

            await _ruleRepository.Delete(rule);

            return Ok(new
            {
                Message = "Sucessfuly deleted rule"
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> UpdateRule(int id, AutoCategorizeRule rule)
        {
            var databaseRule = await _ruleRepository.GetByid(id);
            if (databaseRule == null)
            {
                return BadRequest(new
                {
                    Description = "Problem with rules",
                    Message = "Rule does not exist.",
                    StatusCode = 404,
                });
            }

            var category = await _categoryRepository.GetCategoryBycode(rule.CatCode);
            if (category == null)
            {
                return BadRequest(new
                {
                    Description = "Problem with category",
                    Message = "Category does not exist.",
                    StatusCode = 404,
                });
            }

            databaseRule.Title = rule.Title;
            databaseRule.CatCode = rule.CatCode;
            databaseRule.Predicate = rule.Predicate;

            await _ruleRepository.UpdateRule(databaseRule);

            return Ok(new
            {
                Message = "Sucessfuly updated rule"
            });
        }
    }
}
